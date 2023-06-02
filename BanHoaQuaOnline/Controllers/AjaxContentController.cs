using Microsoft.AspNetCore.Mvc;

namespace BanHoaQuaOnline.Controllers
{
	public class AjaxContentController : Controller
	{
		
		public IActionResult HeaderCart()
		{
			return ViewComponent("HeaderCart");
		}
		public IActionResult HeaderFavourites()
		{
			return ViewComponent("NumberCart");
		}
	}
}
