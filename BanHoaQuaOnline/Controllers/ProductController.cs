using BanHoaQuaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace BanHoaQuaOnline.Controllers
{
    public class ProductController : Controller
    {
        private readonly SieuThiContext _context;
        public ProductController(SieuThiContext context)
        {
            _context = context;
        }
        [Route("shop.html", Name = ("ShopProduct"))]
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 10;
                var lsTinDangs = _context.TbProducts
                    .AsNoTracking()
                    .OrderBy(x => x.DateCreated);
                PagedList<TbProduct> models = new PagedList<TbProduct>(lsTinDangs, pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Route("danhmuc/{Alias}", Name = ("ListProduct"))]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                var pageSize = 10;
                var danhmuc = _context.TbCategories.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);

                var lsTinDangs = _context.TbProducts
                    .AsNoTracking()
                    .Where(x => x.CatllD == danhmuc.CatlId)
                    .OrderByDescending(x => x.DateCreated);
                PagedList<TbProduct> models = new PagedList<TbProduct>(lsTinDangs, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/{Alias}-{id}.html", Name = ("ProductDetails"))]
        public IActionResult Details(int id)
        {
            var product = _context.TbProducts.Include(x => x.CatllDNavigation).FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            var lsProduct = _context.TbProducts
                .AsNoTracking()
                .Where(x => x.CatllD == product.CatllD && x.ProductId != id && x.Active == true)
                .Take(4)
                .OrderByDescending(x => x.DateCreated)
                .ToList();
            ViewBag.SanPham = lsProduct;
            return View(product);
        }
    }
}
