using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanHoaQuaOnline.Models;
using PagedList.Core;

namespace BanHoaQuaOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCustomersController : Controller
    {
        private readonly SieuThiContext _context;

        public AdminCustomersController(SieuThiContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminCustomers
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsCustomers = _context.TbCustomers
                .AsNoTracking()
                
                .OrderByDescending(x => x.CreateDate);
            PagedList<TbCustomer> models = new PagedList<TbCustomer>(lsCustomers, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }
     

        // GET: Admin/AdminCustomers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbCustomers == null)
            {
                return NotFound();
            }

            var tbCustomer = await _context.TbCustomers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (tbCustomer == null)
            {
                return NotFound();
            }

            return View(tbCustomer);
        }

        // GET: Admin/AdminCustomers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FullName,Birhday,Avatar,Address,Email,Phone,LocationId,District,Ward,CreateDate,Password,Salt,LastLogin,Active")] TbCustomer tbCustomer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbCustomer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbCustomer);
        }

        // GET: Admin/AdminCustomers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbCustomers == null)
            {
                return NotFound();
            }

            var tbCustomer = await _context.TbCustomers.FindAsync(id);
            if (tbCustomer == null)
            {
                return NotFound();
            }
            return View(tbCustomer);
        }

        // POST: Admin/AdminCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FullName,Birhday,Avatar,Address,Email,Phone,LocationId,District,Ward,CreateDate,Password,Salt,LastLogin,Active")] TbCustomer tbCustomer)
        {
            if (id != tbCustomer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbCustomer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbCustomerExists(tbCustomer.CustomerId))
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
            return View(tbCustomer);
        }

        // GET: Admin/AdminCustomers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbCustomers == null)
            {
                return NotFound();
            }

            var tbCustomer = await _context.TbCustomers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (tbCustomer == null)
            {
                return NotFound();
            }

            return View(tbCustomer);
        }

        // POST: Admin/AdminCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbCustomers == null)
            {
                return Problem("Entity set 'SieuThiContext.TbCustomers'  is null.");
            }
            var tbCustomer = await _context.TbCustomers.FindAsync(id);
            if (tbCustomer != null)
            {
                _context.TbCustomers.Remove(tbCustomer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbCustomerExists(int id)
        {
          return (_context.TbCustomers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
