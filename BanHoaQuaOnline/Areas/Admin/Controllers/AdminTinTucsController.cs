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
using AspNetCoreHero.ToastNotification.Notyf;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BanHoaQuaOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminTinTucsController : Controller
    {
        private readonly SieuThiContext _context;
        public INotyfService _notyfService { get; }

        public AdminTinTucsController(SieuThiContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

		// GET: Admin/AdminTinTucs
		public IActionResult Index(int? page)
		{
             

			var pageNumber = page == null || page <= 0 ? 1 : page.Value;
			var pageSize = 10;
			var lsTinTucs = _context.TbNews
				.AsNoTracking()
				.OrderByDescending(x => x.PostId);
			PagedList<TbNews> models = new PagedList<TbNews>(lsTinTucs, pageNumber, pageSize);

			ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
        
            return View(models);
		}

		// GET: Admin/AdminTinTucs/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbNews == null)
            {
                return NotFound();
            }

            var tbTinTuc = await _context.TbNews
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tbTinTuc == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
            return View(tbTinTuc);
        }

        // GET: Admin/AdminTinTucs/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
            return View();
        }

        // POST: Admin/AdminTinTucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Scontents,Contents,Thumb,Published,Alias,CreatedDate,Author,AccountId,Tags,CatlId,IsHot,IsNewfeed,MetaKey,MetaDesc,Views")] TbNews tbNews, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (ModelState.IsValid)
            {
                //Xu ly Thumb
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string imageName = Utilities.SEOUrl(tbNews.Title) + extension;
                    tbNews.Thumb = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                }
                if (string.IsNullOrEmpty(tbNews.Thumb)) tbNews.Thumb = "default.jpg";
                tbNews.Alias = Utilities.SEOUrl(tbNews.Title);
                tbNews.CreatedDate = DateTime.Now;


                _context.Add(tbNews);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
            return View(tbNews);
        }

        // GET: Admin/AdminTinTucs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbNews == null)
            {
                return NotFound();
            }

            var tbTinTuc = await _context.TbNews.FindAsync(id);
            if (tbTinTuc == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
            return View(tbTinTuc);
        }

        // POST: Admin/AdminTinTucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Scontents,Contents,Thumb,Published,Alias,CreatedDate,Author,AccountId,Tags,CatlId,IsHot,IsNewfeed,MetaKey,MetaDesc,Views")] TbNews tbNews, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (id != tbNews.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Xu ly Thumb
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string imageName = Utilities.SEOUrl(tbNews.Title) + extension;
                        tbNews.Thumb = await Utilities.UploadFile(fThumb, @"news", imageName.ToLower());
                    }
                    if (string.IsNullOrEmpty(tbNews.Thumb)) tbNews.Thumb = "default.jpg";
                    tbNews.Alias = Utilities.SEOUrl(tbNews.Title);

                    _context.Update(tbNews);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbTinTucExists(tbNews.PostId))
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
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
            return View(tbNews);
        }

        // GET: Admin/AdminTinTucs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbNews == null)
            {
                return NotFound();
            }

            var tbTinTuc = await _context.TbNews
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tbTinTuc == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
            return View(tbTinTuc);
        }

        // POST: Admin/AdminTinTucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbNews == null)
            {
                return Problem("Entity set 'SieuThiContext.TbTinTucs'  is null.");
            }
            var tbTinTuc = await _context.TbNews.FindAsync(id);
            if (tbTinTuc != null)
            {
                _context.TbNews.Remove(tbTinTuc);
            }

         
           
           

            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool TbTinTucExists(int id)
        {
          return (_context.TbNews?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
