using System;
using System.Collections.Generic;

namespace BanHoaQuaOnline.Models;

public partial class TbRole
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? Desciption { get; set; }

    public virtual ICollection<TbAccount> TbAccounts { get; set; } = new List<TbAccount>();
}
