using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanHoaQuaOnline.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;

namespace BanHoaQuaOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminRolesController : Controller
    {
        private readonly SieuThiContext _context;
        public INotyfService _notyfService { get; }
        public AdminRolesController(SieuThiContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        
        // GET: Admin/AdminRoles
        public async Task<IActionResult> Index()
        {
              return _context.TbRoles != null ? 
                          View(await _context.TbRoles.ToListAsync()) :
                          Problem("Entity set 'SieuThiContext.TbRoles'  is null.");
        }

        // GET: Admin/AdminRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbRoles == null)
            {
                return NotFound();
            }

            var tbRole = await _context.TbRoles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (tbRole == null)
            {
                return NotFound();
            }

            return View(tbRole);
        }

        // GET: Admin/AdminRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,RoleName,Desciption")] TbRole tbRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbRole);
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
                
            }
            return View(tbRole);
        }

        // GET: Admin/AdminRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbRoles == null)
            {
                return NotFound();
            }

            var tbRole = await _context.TbRoles.FindAsync(id);
            if (tbRole == null)
            {
                return NotFound();
            }
            return View(tbRole);
        }

        // POST: Admin/AdminRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName,Desciption")] TbRole tbRole)
        {
            if (id != tbRole.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbRole);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbRoleExists(tbRole.RoleId))
                    {
                        _notyfService.Success("Có lỗi xảy ra");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tbRole);
        }

        // GET: Admin/AdminRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbRoles == null)
            {
                return NotFound();
            }

            var tbRole = await _context.TbRoles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (tbRole == null)
            {
                return NotFound();
            }

            return View(tbRole);
        }

        // POST: Admin/AdminRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbRoles == null)
            {
                return Problem("Entity set 'SieuThiContext.TbRoles'  is null.");
            }
            var tbRole = await _context.TbRoles.FindAsync(id);
            if (tbRole != null)
            {
                _context.TbRoles.Remove(tbRole);
                
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa quyền truy cập thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool TbRoleExists(int id)
        {
          return (_context.TbRoles?.Any(e => e.RoleId == id)).GetValueOrDefault();
        }
    }
}
