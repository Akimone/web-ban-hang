using System;
using System.Collections.Generic;

namespace BanHoaQuaOnline.Models;

public partial class TbOrder
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int? TransactStatusId { get; set; }

    public bool Deleted { get; set; }

    public bool Paid { get; set; }

    public int? TotalMoney { get; set; }

    public string? Address { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual TbCustomer? Customer { get; set; }

    public virtual ICollection<TbOrderDetaill> TbOrderDetaills { get; set; } = new List<TbOrderDetaill>();

    public virtual TbTransactStatus? TransactStatus { get; set; }
}
