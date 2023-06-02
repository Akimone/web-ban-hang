using BanHoaQuaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace BanHoaQuaOnline.Controllers
{
    public class BlogController : Controller
    {
        private readonly SieuThiContext _context;
        public BlogController(SieuThiContext context)
        {
            _context = context;
        }
        [Route("blogs.html", Name = ("Blog"))]
        public IActionResult Index(int? page)
        {


            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 6;
            var lsTinTucs = _context.TbNews
                .AsNoTracking()
                .OrderByDescending(x => x.PostId);

            PagedList<TbNews> models = new PagedList<TbNews>(lsTinTucs, pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;      
            return View(models);
        }

        [Route("/tin-tuc/{Alias}-{id}.html", Name = "TinChiTiet")]
        public IActionResult Details(int id)
        {
            var tintuc = _context.TbNews.AsNoTracking().SingleOrDefault(x=>x.PostId == id);
            if (tintuc == null)
            {
                return RedirectToAction("Index");
            }
     
            return View(tintuc);
        }
    }
}
