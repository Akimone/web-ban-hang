using BanHoaQuaOnline.Models;

namespace BanHoaQuaOnline.ModelsView
{
    public class XemDonHang
    {
        public TbOrder? DonHang { get; set; }
        public List<TbOrderDetaill>? ChiTietDonHang { get; set; }
    }
}
