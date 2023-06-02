
using BanHoaQuaOnline.Models;

namespace BanHoaQuaOnline.ModelsView { 

public class CartItem
{
    public TbProduct product { get; set; }
    public int amount { get; set; }
    public double TotalMoney => amount * product.Price.Value;
}

}
