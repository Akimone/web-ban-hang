using BanHoaQuaOnline.Models;

namespace BanHoaQuaOnline.ModelsView
{
    public class ProductHomeVM
    {
        public TbCategory category { get; set; }
        public List<TbProduct> lsProducts { get; set; }
    }
}
