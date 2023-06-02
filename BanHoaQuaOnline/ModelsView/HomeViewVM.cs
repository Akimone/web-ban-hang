using System;
using System.Collections.Generic;
using BanHoaQuaOnline.Models;
namespace BanHoaQuaOnline.ModelsView
{
    public class HomeViewVM
    {
        public List<TbNews> news { get; set; }
        public List<ProductHomeVM>? Products { get; set; }
      
    }
}

