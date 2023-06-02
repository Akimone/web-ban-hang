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

namespace BanHoaQuaOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        public INotyfService _notyfService { get; }

        private readonly SieuThiContext _context;

        public AdminProductsController(SieuThiContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int CatID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<TbProduct> lsProducts = new List<TbProduct>();
            if (CatID != 0)
            {
                lsProducts = _context.TbProducts
                .AsNoTracking()
                .Where(x => x.CatllD == CatID)
                .Include(x => x.CatllDNavigation)
                .OrderBy(x => x.ProductId).ToList();
            }
            else
            {
                lsProducts = _context.TbProducts
                .AsNoTracking()
                .Include(x => x.CatllDNavigation)
                .OrderBy(x => x.ProductId).ToList();
            }
            PagedList<TbProduct> models = new PagedList<TbProduct>(lsProducts.AsQueryable(), pageNumber, pageSize);


            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");

            
            return View(models);
        }
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbProducts == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.CatllDNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Desciption,CatllD,Price,Discount,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,UnitslnStock")] TbProduct tbProduct, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
				tbProduct.ProductName = Utilities.ToTitleCase(tbProduct.ProductName);
				if (fThumb != null)
				{
					string extension = Path.GetExtension(fThumb.FileName);
					string image = Utilities.SEOUrl(tbProduct.ProductName) + extension;
					tbProduct.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
				}
				if (string.IsNullOrEmpty(tbProduct.Thumb)) tbProduct.Thumb = "default.jpg";
				tbProduct.Alias = Utilities.SEOUrl(tbProduct.ProductName);
				tbProduct.DateModified = DateTime.Now;
				tbProduct.DateCreated = DateTime.Now;

				_context.Add(tbProduct);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName", tbProduct.CatllD);
            return View(tbProduct);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbProducts == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts.FindAsync(id);
            if (tbProduct == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName", tbProduct.CatllD);
            return View(tbProduct);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Desciption,CatllD,Price,Discount,Thumb,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Title,Alias,MetaDesc,MetaKey,UnitslnStock")] TbProduct Product, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (id != Product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product.ProductName = Utilities.ToTitleCase(Product.ProductName);
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(Product.ProductName) + extension;
                        Product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(Product.Thumb)) Product.Thumb = "default.jpg";
                    Product.Alias = Utilities.SEOUrl(Product.ProductName);
                    Product.DateModified = DateTime.Now;

                    _context.Update(Product);
                    _notyfService.Success("Cập nhật thành công");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbProductExists(Product.ProductId))
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
            ViewData["DanhMuc"] = new SelectList(_context.TbCategories, "CatlId", "CatName", Product.CatllD);
            return View(Product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbProducts == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.CatllDNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbProducts == null)
            {
                return Problem("Entity set 'SieuThiContext.TbProducts'  is null.");
            }
            var tbProduct = await _context.TbProducts.FindAsync(id);
            if (tbProduct != null)
            {
                _context.TbProducts.Remove(tbProduct);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa sản phẩm thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool TbProductExists(int id)
        {
          return (_context.TbProducts?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
