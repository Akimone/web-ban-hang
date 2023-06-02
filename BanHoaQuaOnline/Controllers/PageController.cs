using BanHoaQuaOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace BanHoaQuaOnline.Controllers
{
    public class PageController : Controller
    {
        private readonly SieuThiContext _context;
        public PageController(SieuThiContext context)
        {
            _context = context;
        }
       

        [Route("/page/{Alias}", Name = "PageChiTiet")]
        public IActionResult Details(string Alias)
        {
            if (string.IsNullOrEmpty(Alias)) return RedirectToAction("Index","Home");
            var page = _context.TbPages.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
            if (page == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(page);
        }
    }
}
