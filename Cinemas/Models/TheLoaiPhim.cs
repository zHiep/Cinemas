using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class TheLoaiPhim
{
    public int IdtheLoai { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}
