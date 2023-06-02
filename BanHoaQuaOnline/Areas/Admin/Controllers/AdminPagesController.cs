using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanHoaQuaOnline.Models;
using PagedList.Core;
using BanHoaQuaOnline.Helpper;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;

namespace BanHoaQuaOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPagesController : Controller
    {
        public INotyfService _notyfService { get; }
        private readonly SieuThiContext _context;

        public AdminPagesController(SieuThiContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminPages
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsPages = _context.TbPages
                .AsNoTracking()
                .OrderByDescending(x => x.PageId);
            PagedList<TbPage> models = new PagedList<TbPage>(lsPages, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: Admin/AdminPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbPages == null)
            {
                return NotFound();
            }

            var tbPage = await _context.TbPages
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (tbPage == null)
            {
                return NotFound();
            }

            return View(tbPage);
        }

        // GET: Admin/AdminPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PageId,PageName,Contents,Thumb,Published,Title,MetaDesc,MetaKey,Alias,CreateDate,Ordering")] TbPage tbPage, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(tbPage.PageName) + extension;
                    tbPage.Thumb = await Utilities.UploadFile(fThumb, @"pages", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(tbPage.Thumb)) tbPage.Thumb = "default.jpg";
                tbPage.Alias = Utilities.SEOUrl(tbPage.PageName);
                tbPage.CreateDate = DateTime.Now;

                _context.Add(tbPage);

                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(tbPage);
        }

        // GET: Admin/AdminPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbPages == null)
            {
                return NotFound();
            }

            var tbPage = await _context.TbPages.FindAsync(id);
            if (tbPage == null)
            {
                return NotFound();
            }
            return View(tbPage);
        }

        // POST: Admin/AdminPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageId,PageName,Contents,Thumb,Published,Title,MetaDesc,MetaKey,Alias,CreateDate,Ordering")] TbPage tbPage, Microsoft.AspNetCore.Http.IFormFile? file)
        {
            if (id != tbPage.PageId)
            {
                return NotFound();
            }
            var currentTbPage = await _context.TbPages.FindAsync(id);
            if (currentTbPage == null)
            {
                return NotFound();
            }
            else
            {
                tbPage.Thumb = currentTbPage.Thumb;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        string extension = Path.GetExtension(file.FileName);
                        string imageName = Utilities.SEOUrl(tbPage.PageName) + extension;
                        tbPage.Thumb = await Utilities.UploadFile(file, @"pages", imageName.ToLower());
                    }
                    currentTbPage.PageName = tbPage.PageName;
                    currentTbPage.Ordering = tbPage.Ordering;
                    currentTbPage.Thumb = tbPage.Thumb;
                    currentTbPage.Published = tbPage.Published;
                    currentTbPage.Title = tbPage.Title;
                    currentTbPage.MetaKey = tbPage.MetaKey;
                    currentTbPage.MetaDesc = tbPage.MetaDesc;
                    currentTbPage.Contents = tbPage.Contents;

                    tbPage.Alias = Utilities.SEOUrl(tbPage.PageName);
                    _context.Update(currentTbPage);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbPageExists(tbPage.PageId))
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
            return View(tbPage);
        }

        // GET: Admin/AdminPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbPages == null)
            {
                return NotFound();
            }

            var tbPage = await _context.TbPages
                .FirstOrDefaultAsync(m => m.PageId == id);
            if (tbPage == null)
            {
                return NotFound();
            }

            return View(tbPage);
        }

        // POST: Admin/AdminPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbPages == null)
            {
                return Problem("Entity set 'SieuThiContext.TbPages'  is null.");
            }
            var tbPage = await _context.TbPages.FindAsync(id);
            if (tbPage != null)
            {
                _context.TbPages.Remove(tbPage);
            }
           
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa sản phẩm thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool TbPageExists(int id)
        {
          return (_context.TbPages?.Any(e => e.PageId == id)).GetValueOrDefault();
        }
    }
}
