using Microsoft.AspNetCore.Mvc;

namespace BanHoaQuaOnline.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
