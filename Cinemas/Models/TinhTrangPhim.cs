using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class TinhTrangPhim
{
    public int IdtinhTrangPhim { get; set; }

    public string TenTinhTrang { get; set; } = null!;

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}
