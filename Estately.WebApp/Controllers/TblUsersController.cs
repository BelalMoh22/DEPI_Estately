using Microsoft.AspNetCore.Mvc.Rendering;
namespace Estately.WebApp.Controllers
{
    //public class TblUsersController : Controller
    //{
    //    private readonly IServiceUser _service;
    //    private readonly AppDBContext _context;
    //    public TblUsersController(IServiceUser service, AppDBContext context)
    //    {
    //        _service = service;
    //        _context = context;
    //    }

    //    // GET: TblUsers
    //    public async Task<IActionResult> Index()
    //    {
    //        var user = await _service.PaginationUserAsync();
    //        return View(user);
    //    }

    //    // GET: TblUsers/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var user = await _service.GetUserByIDAsync(id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(user);
    //    }

    //    // GET: TblUsers/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName");
    //        return View();
    //    }

    //    // POST: TblUsers/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create(TblUser user)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _service.AddUserAsync(user);
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName", user.UserTypeID);
    //        return View(user);
    //    }

    //    // GET: TblUsers/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var user = await _service.GetUserByIDAsync(id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName", user.UserTypeID);
    //        return View(user);
    //    }

    //    // POST: TblUsers/Edit/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, TblUser user)
    //    {
    //        if (id != user.UserID)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                await _service.UpdateUserAsync(user);
    //            }
    //            catch (Exception ex)
    //            {
    //                return BadRequest(ex.Message.ToString());
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName", user.UserTypeID);
    //        return View(user);
    //    }

    //    // GET: TblUsers/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var user = await _service.GetUserByIDAsync(id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(user);
    //    }

    //    // POST: TblUsers/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var user = await _service.GetUserByIDAsync(id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        await _service.DeleteUserAsync(id);
    //        return RedirectToAction(nameof(Index));
    //    }
    //}
    public class TblUsersController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public TblUsersController(IServiceUser serviceUser)
        {
            _serviceUser = serviceUser;
        }
        // =======================================================
        // INDEX (LIST + SEARCH + PAGINATION)
        // =======================================================
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            var model = await _serviceUser.GetUsersPagedAsync(page, pageSize, search);
            return View(model);
        }
        // =======================================================
        // DETAILS (VIEW USER INFO)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _serviceUser.GetUserByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // CREATE
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadUserTypesDropdown();
            return View(new UserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadUserTypesDropdown();
                return View(model);
            }
            await _serviceUser.CreateUserAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // EDIT
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _serviceUser.GetUserByIdAsync(id);
            if (model == null)
                return NotFound();

            await LoadUserTypesDropdown();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadUserTypesDropdown();
                return View(model);
            }

            // check existence using the existing service method
            var existing = await _serviceUser.GetUserByIdAsync(model.UserID);
            if (existing == null)
                return NotFound();

            await _serviceUser.UpdateUserAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // DELETE (SOFT DELETE)
        // =======================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceUser.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // =======================================================
        // TOGGLE ACTIVE / INACTIVE
        // =======================================================
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ToggleStatus(int id)
        //{
        //    await _serviceUser.ToggleStatusAsync(id);
        //    return RedirectToAction(nameof(Index));
        //}

        // =======================================================
        // ASSIGN ROLE
        // =======================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(int userId, int userTypeId)
        {
            await _serviceUser.AssignRoleAsync(userId, userTypeId);
            return RedirectToAction("Edit", new { id = userId });
        }

        // =======================================================
        // PRIVATE HELPER: LOAD DROPDOWN FOR USER TYPES
        // =======================================================
        private async Task LoadUserTypesDropdown()
        {
            var userTypes = await _serviceUser.GetAllUserTypesAsync();

            ViewBag.UserTypes = new SelectList(
                userTypes,
                "UserTypeID",
                "UserTypeName"
            );
        }
    }
}