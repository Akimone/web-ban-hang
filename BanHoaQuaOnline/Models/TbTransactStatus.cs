using System;
using System.Collections.Generic;

namespace BanHoaQuaOnline.Models;

public partial class TbTransactStatus
{
    public int TransactStatusId { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbOrder> TbOrders { get; set; } = new List<TbOrder>();
}
