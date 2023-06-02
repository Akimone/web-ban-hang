using System;
using System.Collections.Generic;

namespace BanHoaQuaOnline.Models;

public partial class TbOrderDetaill
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? OrderNumber { get; set; }

    public int? Amount { get; set; }

    public int? Discount { get; set; }

    public int? TotalMoney { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? Price { get; set; }

    public virtual TbOrder? Order { get; set; }
    public virtual TbProduct? Product { get; set; }
}
