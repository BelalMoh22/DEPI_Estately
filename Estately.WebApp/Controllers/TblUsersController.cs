using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Estately.Core.Entities;
using Estately.Infrastructure.Data;

namespace Estately.WebApp.Controllers
{
    public class TblUsersController : Controller
    {
        private readonly IServiceUser _service;
        public TblUsersController(IServiceUser service)
        {
            _service = service;
        }

        // GET: TblUsers
        public async Task<IActionResult> Index()
        {
            var user = await _service.PaginationUserAsync();
            return View(user);
        }

        // GET: TblUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByIDAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: TblUsers/Create
        public IActionResult Create()
        {
            //ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName");
            return View();
        }

        // POST: TblUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblUser user)
        {
            if (ModelState.IsValid)
            {
                _service.AddUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName", user.UserTypeID);
            return View(user);
        }

        // GET: TblUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByIDAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            //ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName", user.UserTypeID);
            return View(user);
        }

        // POST: TblUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TblUser user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.UpdateUserAsync(user);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message.ToString());
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["UserTypeID"] = new SelectList(_context.LkpUserTypes, "UserTypeID", "UserTypeName", user.UserTypeID);
            return View(user);
        }

        // GET: TblUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserByIDAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: TblUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _service.GetUserByIDAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _service.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
