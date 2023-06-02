using BanHoaQuaOnline.Extension;
using BanHoaQuaOnline.ModelsView;
using Microsoft.AspNetCore.Mvc;

namespace BanHoaQuaOnline.Controllers.Components
{
	public class HeaderCartViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
			return View(cart);
		}
	}
}
