using Estately.Services.Interfaces;
using Estately.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Estately.Web.Controllers
{
    public class TblPropertiesController : Controller
    {
        private readonly IServiceProperty _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public TblPropertiesController(IServiceProperty service, IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _env = env;
        }

        // ---------------------------------------------------------
        // 1. Index
        // ---------------------------------------------------------
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _service.GetPropertiesPagedAsync(page, pageSize, search);
            return View(model);
        }

        // ---------------------------------------------------------
        // 2. Create (GET)
        // ---------------------------------------------------------
        public async Task<IActionResult> Create()
        {
            var vm = await BuildPropertyViewModelAsync(new PropertyViewModel());
            return View(vm);
        }

        // ---------------------------------------------------------
        // 3. Create (POST)
        // ---------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await BuildPropertyViewModelAsync(vm);
                return View(vm);
            }

            await HandleImageUpload(vm);

            await _service.CreatePropertyAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------
        // 4. Edit (GET)
        // ---------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetPropertyByIdAsync(id);
            if (vm == null)
                return NotFound();

            vm = await BuildPropertyViewModelAsync(vm);

            return View(vm);
        }

        // ---------------------------------------------------------
        // 5. Edit (POST)
        // ---------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await BuildPropertyViewModelAsync(vm);
                return View(vm);
            }

            await HandleImageUpload(vm);

            if (vm.ImagesToDelete != null)
            {
                foreach (var imgId in vm.ImagesToDelete)
                    await DeleteImageFromDiskAndDb(imgId);
            }

            await _service.UpdatePropertyAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------
        // 6. Delete
        // ---------------------------------------------------------
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeletePropertyAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ---------------------------------------------------------
        // IMAGE UPLOAD HANDLER
        // ---------------------------------------------------------
        private async Task HandleImageUpload(PropertyViewModel vm)
        {
            if (vm.UploadedFiles == null || vm.UploadedFiles.Count == 0)
                return;

            vm.Images ??= new List<PropertyImageViewModel>();

            string folderPath = Path.Combine(_env.WebRootPath, "Images", "Properties");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var file in vm.UploadedFiles)
            {
                string ext = Path.GetExtension(file.FileName);
                string uniqueName = $"{Guid.NewGuid()}{ext}";
                string fullPath = Path.Combine(folderPath, uniqueName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                vm.Images.Add(new PropertyImageViewModel
                {
                    ImagePath = $"Images/Properties/{uniqueName}",
                    UploadedDate = DateTime.Now
                });
            }
        }

        // ---------------------------------------------------------
        // DELETE IMAGE FROM DISK + DB
        // ---------------------------------------------------------
        private async Task DeleteImageFromDiskAndDb(int imageId)
        {
            var img = await _unitOfWork.PropertyImageRepository.GetByIdAsync(imageId);
            if (img == null) return;

            string fullPath = Path.Combine(_env.WebRootPath, img.ImagePath);

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            await _unitOfWork.PropertyImageRepository.DeleteAsync(img.ImageID);
            await _unitOfWork.CompleteAsync();
        }

        // ---------------------------------------------------------
        // Build Strongly Typed VM (NO VIEWBAG)
        // ---------------------------------------------------------
        private async Task<PropertyViewModel> BuildPropertyViewModelAsync(PropertyViewModel vm)
        {
            vm.AllFeatures = await _service.GetAllFeaturesAsync();
            vm.PropertyTypes = await _service.GetAllPropertyTypesAsync();
            vm.Statuses = await _service.GetAllStatusesAsync();
            vm.Developers = await _service.GetAllDevelopersAsync();
            vm.Zones = await _service.GetAllZonesAsync();

            // Load Agents strongly typed
            var employees = await _unitOfWork.EmployeeRepository.ReadAllIncluding("JobTitle");
            employees = employees.Where(e => e.JobTitle?.JobTitleName == "Sales Agent");

            vm.Agents = employees
            .Select(a => new EmployeeViewModel
            { 
                EmployeeID = a.EmployeeID,
                FullName = $"{a.FirstName} {a.LastName}"
            })
            .ToList();

            return vm;
        }
    }
}
