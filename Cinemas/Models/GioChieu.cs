using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class GioChieu
{
    public int IdgioChieu { get; set; }

    public TimeSpan GioChieu1 { get; set; }

    public virtual ICollection<LichChieuPhim> LichChieuPhims { get; set; } = new List<LichChieuPhim>();
}
