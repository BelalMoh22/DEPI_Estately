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

        // --------------------------- FIXED ZONES API ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllZones()
        {
            var zones = await _service.GetAllZonesAsync();
            var result = zones
                .Select(z => new {
                    id = z.ZoneId,
                    name = z.ZoneName,
                    displayName = !string.IsNullOrWhiteSpace(z.City) ? $"{z.ZoneName}, {z.City}" : z.ZoneName
                })
                .OrderBy(z => z.name)
                .ToList();
            return Json(result);
        }

        // --------------------------- CITIES API ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _unitOfWork.CityRepository.ReadAllAsync();
            var result = cities
                .Where(c => !string.IsNullOrWhiteSpace(c.CityName))
                .Select(c => new { id = c.CityID, name = c.CityName })
                .OrderBy(c => c.name)
                .ToList();
            return Json(result);
        }

        // --------------------------- FIXED AREAS API ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllAreas()
        {
            var zones = await _service.GetAllZonesAsync();
            var result = zones
                .Select(z => new {
                    id = z.ZoneId,
                    name = z.ZoneName,
                    cityName = z.City ?? "",
                    displayName = !string.IsNullOrWhiteSpace(z.City) ? $"{z.ZoneName}, {z.City}" : z.ZoneName
                })
                .OrderBy(z => z.name)
                .ToList();
            return Json(result);
        }

        // --------------------------- DEVELOPERS API ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllDevelopers()
        {
            var developers = await _unitOfWork.DeveloperProfileRepository.ReadAllAsync();
            var result = developers
                .Where(d => !string.IsNullOrWhiteSpace(d.DeveloperName))
                .Select(d => new { id = d.DeveloperProfileID, name = d.DeveloperName })
                .OrderBy(d => d.name)
                .ToList();
            return Json(result);
        }

        // --------------------------- AMENITIES API ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllAmenities()
        {
            var features = await _unitOfWork.PropertyFeatureRepository.ReadAllAsync();
            var result = features
                .Where(f => !string.IsNullOrWhiteSpace(f.FeatureName))
                .Select(f => new { id = f.FeatureID, name = f.FeatureName })
                .OrderBy(f => f.name)
                .ToList();
            return Json(result);
        }

        // --------------------------- SUGGESTIONS API ---------------------------
        [HttpGet]
        public async Task<IActionResult> Suggestions(string category, string term)
        {
            if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(term))
            {
                return Json(new List<object>());
            }

            term = term.ToLower().Trim();
            var suggestions = new List<object>();

            switch (category.ToLower())
            {
                case "zones":
                    var zones = await _unitOfWork.ZoneRepository.ReadAllAsync();
                    suggestions = zones
                        .Where(z => z.ZoneName != null && z.ZoneName.ToLower().Contains(term))
                        .Take(10)
                        .Select(z => new { id = z.ZoneID.ToString(), name = z.ZoneName, text = z.ZoneName })
                        .Cast<object>()
                        .ToList();
                    break;

                case "developers":
                    var developers = await _unitOfWork.DeveloperProfileRepository.ReadAllAsync();
                    suggestions = developers
                        .Where(d => d.DeveloperTitle != null && d.DeveloperTitle.ToLower().Contains(term))
                        .Take(10)
                        .Select(d => new { id = d.DeveloperProfileID.ToString(), name = d.DeveloperTitle, text = d.DeveloperTitle })
                        .Cast<object>()
                        .ToList();
                    break;

                case "cities":
                    var cities = await _unitOfWork.CityRepository.ReadAllAsync();
                    suggestions = cities
                        .Where(c => c.CityName != null && c.CityName.ToLower().Contains(term))
                        .Take(10)
                        .Select(c => new { id = c.CityID.ToString(), name = c.CityName, text = c.CityName })
                        .Cast<object>()
                        .ToList();
                    break;
            }

            return Json(suggestions);
        }

        // --------------------------- CREATE GET ---------------------------
        public async Task<IActionResult> Create()
        {
            var vm = new PropertyViewModel
            {
                SelectedFeatures = new List<int>()
            };

            vm = await BuildPropertyViewModelAsync(vm);

            // Ensure status dropdown starts with the "-- Select --" placeholder
            vm.StatusId = 0;

            return View(vm);
        }

        // --------------------------- CREATE POST ---------------------------
        //    if (vm.Price <= 0)
        //    {
        //        ModelState.AddModelError("Price", "Price is required.");
        //    }
        //    if (vm.Area <= 0)
        //    {
        //        ModelState.AddModelError("Area", "Area is required.");
        //    }
        //    if (vm.PropertyTypeID <= 0)
        //    {
        //        ModelState.AddModelError("PropertyTypeID", "Property Type is required.");
        //    }
        //    if (vm.StatusId <= 0)
        //    {
        //        ModelState.AddModelError("StatusId", "Property Status is required.");
        //    }
        //    if (vm.ZoneID <= 0)
        //    {
        //        ModelState.AddModelError("ZoneID", "Zone is required.");
        //    }
        //    if (vm.UploadedFiles == null || vm.UploadedFiles.Count < 3)
        //    {
        //        ModelState.AddModelError("UploadedFiles", "You must upload at least 3 images.");

        //        // reload dropdowns
        //        vm = await BuildPropertyViewModelAsync(vm);
        //        return View(vm);
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        vm = await BuildPropertyViewModelAsync(vm);
        //        return View(vm);
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyViewModel vm)
        {
            // -----------------------------
            // 🔥 Add ALL validations
            // -----------------------------
            if (vm.Price <= 0)
                ModelState.AddModelError("Price", "Price is required.");

            if (vm.Area <= 0)
                ModelState.AddModelError("Area", "Area is required.");

            if (vm.PropertyTypeID <= 0)
                ModelState.AddModelError("PropertyTypeID", "Property Type is required.");

            if (vm.StatusId <= 0)
                ModelState.AddModelError("StatusId", "Property Status is required.");

            if (vm.ZoneID <= 0)
                ModelState.AddModelError("ZoneID", "Zone is required.");

            if (vm.UploadedFiles == null || vm.UploadedFiles.Count < 1)
                ModelState.AddModelError("UploadedFiles", "You must upload at least 1 image.");

            // -----------------------------
            // 🔥 Duplicate address validation
            // -----------------------------
            if (!string.IsNullOrWhiteSpace(vm.Address))
            {
                var allProps = await _unitOfWork.PropertyRepository.ReadAllAsync();
                var normalizedAddress = vm.Address.Trim().ToLower();

                bool duplicate = allProps.Any(p => p.IsDeleted == false &&
                    p.ZoneID == vm.ZoneID &&
                    (p.Address ?? string.Empty).Trim().ToLower() == normalizedAddress);

                if (duplicate)
                {
                    ModelState.AddModelError("Address", "A property with this address already exists.");
                }
            }

            // -----------------------------
            // 🔥 If validation failed → reload dropdowns + return view
            // -----------------------------
            if (!ModelState.IsValid)
            {
                vm = await BuildPropertyViewModelAsync(vm); // reload dropdowns
                return View(vm);
            }

            // -----------------------------
            // 🔥 Valid → upload images + save
            // -----------------------------
            await HandleImageUpload(vm);
            await _service.CreatePropertyAsync(vm);
            TempData["Success"] = "Property created successfully.";
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
            var allImages = await _unitOfWork.PropertyImageRepository.ReadAllAsync();
            var existingImages = allImages.Where(i => i.PropertyID == vm.PropertyID).ToList();

            var imagesMarkedForDeletion = vm.ImagesToDelete?.Count ?? 0;
            var newUploads = vm.UploadedFiles?.Count ?? 0;
            var finalImageCount = existingImages.Count - imagesMarkedForDeletion + newUploads;

            if (finalImageCount < 1)
            {
                ModelState.AddModelError("UploadedFiles", "You must have at least 1 image.");
            }

            // -----------------------------
            // 🔥 Duplicate address validation on Edit
            // -----------------------------
            if (!string.IsNullOrWhiteSpace(vm.Address))
            {
                var allProps = await _unitOfWork.PropertyRepository.ReadAllAsync();
                var normalizedAddress = vm.Address.Trim().ToLower();

                bool duplicate = allProps.Any(p => p.IsDeleted == false &&
                    p.PropertyID != vm.PropertyID &&
                    p.ZoneID == vm.ZoneID &&
                    (p.Address ?? string.Empty).Trim().ToLower() == normalizedAddress);

                if (duplicate)
                {
                    ModelState.AddModelError("Address", "A property with this address already exists.");
                }
            }

            if (!ModelState.IsValid)
            {
                vm = await BuildPropertyViewModelAsync(vm);

                vm.Images = existingImages
                    .Where(i => vm.ImagesToDelete == null || !vm.ImagesToDelete.Contains(i.ImageID))
                    .Select(i => new PropertyImageViewModel
                    {
                        ImageID = i.ImageID,
                        ImagePath = i.ImagePath,
                        UploadedDate = i.UploadedDate
                    }).ToList();

                return View(vm);
            }

            await HandleImageUpload(vm);

            if (vm.ImagesToDelete != null)
            {
                foreach (var imgId in vm.ImagesToDelete)
                    await DeleteImageFromDiskAndDb(imgId);
            }

            await _service.UpdatePropertyAsync(vm);
            TempData["Success"] = "Property updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // --------------------------- DETAILS ---------------------------
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _service.GetPropertyByIdAsync(id);
            if (vm == null)
                return NotFound();

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
            if (!string.Equals(model.StatusName, "Unavailable", StringComparison.OrdinalIgnoreCase))
            {
                //ModelState.AddModelError(string.Empty, "Property can only be deleted when its status is 'Unavailable'.");
                TempData["Error"] = "Cannot delete this property when its status is 'Unavailable'.";
                //return View("Delete", model);
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _service.DeletePropertyAsync(id);
            TempData["Success"] = "Property deleted successfully.";
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
        // BUILD VIEWMODEL
        // ---------------------------------------------------------------
        private async Task<PropertyViewModel> BuildPropertyViewModelAsync(PropertyViewModel vm)
        {
            vm.AllFeatures = await _service.GetAllFeaturesAsync();
            vm.PropertyTypes = await _service.GetAllPropertyTypesAsync();
            vm.Statuses = await _service.GetAllStatusesAsync();
            vm.Developers = await _service.GetAllDevelopersAsync();
            vm.Zones = await _service.GetAllZonesAsync();

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