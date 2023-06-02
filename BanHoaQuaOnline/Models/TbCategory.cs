using System;
using System.Collections.Generic;

namespace BanHoaQuaOnline.Models;

public partial class TbCategory
{
    public int CatlId { get; set; }

    public string? CatName { get; set; }

    public string? Desciption { get; set; }

    public int? Ordering { get; set; }

    public bool Published { get; set; }

    public string? Thumb { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? MetaDesc { get; set; }

    public string? MetaKey { get; set; }

    public string? Cover { get; set; }

    public string? SchemaMarkup { get; set; }

    public virtual ICollection<TbNews> TbNews { get; set; } = new List<TbNews>();

    public virtual ICollection<TbProduct> TbProducts { get; set; } = new List<TbProduct>();
}
