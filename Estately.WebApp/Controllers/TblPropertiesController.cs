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
        // --------------------------- INDEX ---------------------------
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var vm = await _service.GetPropertiesPagedAsync(page, pageSize, search);
            return View(vm);
        }

        // --------------------------- CREATE GET ---------------------------
        public async Task<IActionResult> Create()
        {
            var vm = new PropertyViewModel
            {
                SelectedFeatures = new List<int>()
            };

            vm = await BuildPropertyViewModelAsync(vm);

            return View(vm);
        }
        // --------------------------- CREATE POST ---------------------------
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

        // --------------------------- EDIT GET ---------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetPropertyByIdAsync(id);
            if (vm == null) return NotFound();

            vm = await BuildPropertyViewModelAsync(vm);

            return View(vm);
        }

        // --------------------------- EDIT POST ---------------------------
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

        // --------------------------- DETAILS ---------------------------
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _service.GetPropertyByIdAsync(id);
            if (vm == null)
                return NotFound();

            // Ensure features and lookup collections are populated for the view
            vm = await BuildPropertyViewModelAsync(vm);

            return View(vm);
        }

        // --------------------------- DELETE ---------------------------
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _service.GetPropertyByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: TblDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _service.GetPropertyByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            await _service.DeletePropertyAsync(id);
            return RedirectToAction(nameof(Index));
        }


        // ---------------------------------------------------------------
        // IMAGE HANDLING
        // ---------------------------------------------------------------
        private async Task HandleImageUpload(PropertyViewModel vm)
        {
            if (vm.UploadedFiles == null || vm.UploadedFiles.Count == 0)
                return;

            vm.Images ??= new List<PropertyImageViewModel>();

            string folder = Path.Combine(_env.WebRootPath, "Images", "Properties");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var file in vm.UploadedFiles)
            {
                string ext = Path.GetExtension(file.FileName);
                string originalName = Path.GetFileNameWithoutExtension(file.FileName);
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string safeBaseName = string.IsNullOrWhiteSpace(originalName) ? "image" : originalName.Trim();
                // Final file name: OriginalName_yyyyMMddHHmmssfff.ext
                string name = $"{safeBaseName}_{timeStamp}{ext}";
                string path = Path.Combine(folder, name);

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                vm.Images.Add(new PropertyImageViewModel
                {
                    ImagePath = $"Images/Properties/{name}",
                    UploadedDate = DateTime.Now
                });
            }
        }
        private async Task DeleteImageFromDiskAndDb(int id)
        {
            var img = await _unitOfWork.PropertyImageRepository.GetByIdAsync(id);
            if (img == null) return;

            string fullPath = Path.Combine(_env.WebRootPath, img.ImagePath);
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            await _unitOfWork.PropertyImageRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        // ---------------------------------------------------------------
        // BUILD VIEWMODEL (NO VIEWBAG)
        // ---------------------------------------------------------------
        private async Task<PropertyViewModel> BuildPropertyViewModelAsync(PropertyViewModel vm)
        {
            vm.AllFeatures = await _service.GetAllFeaturesAsync();
            vm.PropertyTypes = await _service.GetAllPropertyTypesAsync();
            vm.Statuses = await _service.GetAllStatusesAsync();
            vm.Developers = await _service.GetAllDevelopersAsync();
            vm.Zones = await _service.GetAllZonesAsync();

            // Strong typed Agents list
            var employees = await _unitOfWork.EmployeeRepository.ReadAllIncluding("JobTitle");
            employees = employees.Where(e => e.JobTitle?.JobTitleName == "Sales Agent");

            vm.Agents = employees.Select(e => new EmployeeViewModel
            {
                EmployeeID = e.EmployeeID,
                FullName = $"{e.FirstName} {e.LastName}"
            }).ToList();

            return vm;
        }
    }
}