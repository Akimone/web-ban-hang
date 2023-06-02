using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanHoaQuaOnline.Models;

using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Abstractions;
using BanHoaQuaOnline.Extension;
using BanHoaQuaOnline.Helpper;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace BanHoaQuaOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminAccountsController : Controller
    {
        private readonly SieuThiContext _context;
        public INotyfService _notyfService { get; }
        public AdminAccountsController(SieuThiContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

       
        // GET: Admin/AdminAccounts
        public async Task<IActionResult> Index()
        {
            ViewData["QuyenTruyCap"] = new SelectList(_context.TbRoles, "RoleId", "Desciption");
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Hoạt động", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Khóa", Value = "0" });
            ViewData["lsTrangThai"] = lsTrangThai;

            var sieuThiContext = _context.TbAccounts.Include(t => t.Role);
            return View(await sieuThiContext.ToListAsync());
        }

        // GET: Admin/AdminAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbAccounts == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (tbAccount == null)
            {
                return NotFound();
            }

            return View(tbAccount);
        }

        // GET: Admin/AdminAccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId");
            return View();
        }

        // POST: Admin/AdminAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Phone,Email,Password,Salt,Active,FullName,RoleId,LastLogin,CreateDate")] TbAccount tbAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId", tbAccount.RoleId);
            return View(tbAccount);
        }

        // GET: Admin/AdminAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbAccounts == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts.FindAsync(id);
            if (tbAccount == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId", tbAccount.RoleId);
            return View(tbAccount);
        }

        // POST: Admin/AdminAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Phone,Email,Password,Salt,Active,FullName,RoleId,LastLogin,CreateDate")] TbAccount tbAccount)
        {
            if (id != tbAccount.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbAccountExists(tbAccount.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.TbRoles, "RoleId", "RoleId", tbAccount.RoleId);
            return View(tbAccount);
        }

        // GET: Admin/AdminAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbAccounts == null)
            {
                return NotFound();
            }

            var tbAccount = await _context.TbAccounts
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (tbAccount == null)
            {
                return NotFound();
            }

            return View(tbAccount);
        }

        // POST: Admin/AdminAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbAccounts == null)
            {
                return Problem("Entity set 'SieuThiContext.TbAccounts'  is null.");
            }
            var tbAccount = await _context.TbAccounts.FindAsync(id);
            if (tbAccount != null)
            {
                _context.TbAccounts.Remove(tbAccount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbAccountExists(int id)
        {
          return (_context.TbAccounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
      
    }
}
