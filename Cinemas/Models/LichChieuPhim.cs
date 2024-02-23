using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class LichChieuPhim
{
    public int IdlichChieu { get; set; }

    public DateTime NgayChieu { get; set; }

    public int Idphim { get; set; }

    public int IdgioChieu { get; set; }

    public int Idphong { get; set; }

    public int GheTrong { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual GioChieu IdgioChieuNavigation { get; set; } = null!;

    public virtual Phim IdphimNavigation { get; set; } = null!;

    public virtual Phong IdphongNavigation { get; set; } = null!;
}
