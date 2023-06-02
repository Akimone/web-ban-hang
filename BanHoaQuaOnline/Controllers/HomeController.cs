using BanHoaQuaOnline.Models;
using BanHoaQuaOnline.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BanHoaQuaOnline.Controllers
{
    public class HomeController : Controller
    {
		private readonly SieuThiContext _context;
		private readonly ILogger<HomeController> _logger;
		
		public HomeController(ILogger<HomeController> logger, SieuThiContext context)
        {
            _logger = logger;
			_context = context;
		}
		public IActionResult Index()
        {
			HomeViewVM model = new HomeViewVM();

			var lsProducts = _context.TbProducts.AsNoTracking()
				.Where(x => x.Active == true && x.HomeFlag == true)
				.OrderByDescending(x => x.DateCreated)
				.ToList();

            

            List<ProductHomeVM> lsProductViews = new List<ProductHomeVM>();
			var lsCats = _context.TbCategories
				.AsNoTracking()
				.Where(x => x.Published == true)
				.OrderByDescending(x => x.Ordering)
				.ToList();

			foreach (var item in lsCats)
			{
				ProductHomeVM productHome = new ProductHomeVM();
				productHome.category = item;
				productHome.lsProducts = lsProducts.Where(x => x.CatllD == item.CatlId).ToList();
				lsProductViews.Add(productHome);

				

				var TinTuc = _context.TbNews
					.AsNoTracking()
					.Where(x => x.Published == true && x.IsNewfeed == true)
					.OrderByDescending(x => x.CreatedDate)
					.Take(3)
					.ToList();

				model.Products = lsProductViews;
				
				model.news = TinTuc;
				ViewBag.AllProducts = lsProducts;
			}
			return View(model);
		}

        public IActionResult Privacy()
        {
            return View();
        }
		[Route("lien-he.html", Name = "Contact")]
		public IActionResult Contact()
        {
            return View();
        }
		[Route("gioi-thieu.html", Name = "About")]
		public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}