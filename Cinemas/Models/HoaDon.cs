using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class HoaDon
{
    public int IdhoaDon { get; set; }

    public int Idkh { get; set; }

    public int IdlichChieu { get; set; }

    public DateTime NgayMuaVe { get; set; }

    public int SoLuongGhe { get; set; }

    public string TenGhe { get; set; } = null!;

    public decimal TongTien { get; set; }

    public virtual KhachHang IdkhNavigation { get; set; } = null!;

    public virtual LichChieuPhim IdlichChieuNavigation { get; set; } = null!;
}
