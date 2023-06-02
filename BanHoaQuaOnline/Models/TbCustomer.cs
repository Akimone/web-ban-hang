using System;
using System.Collections.Generic;

namespace BanHoaQuaOnline.Models;

public partial class TbCustomer
{
    public int CustomerId { get; set; }

    public string? FullName { get; set; }

    public DateTime? Birhday { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<TbOrder> TbOrders { get; set; } = new List<TbOrder>();
}
