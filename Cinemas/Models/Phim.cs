using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class Phim
{
    public int Idphim { get; set; }

    public int IdtheLoai { get; set; }

    public int IdtinhTrangPhim { get; set; }

    public string Img { get; set; } = null!;

    public string TenPhim { get; set; } = null!;

    public int ThoiLuong { get; set; }

    public decimal Gia { get; set; }

    public DateTime NgayKhoiChieu { get; set; }

    public string DaoDien { get; set; } = null!;

    public string DienVien { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public virtual TheLoaiPhim IdtheLoaiNavigation { get; set; } = null!;

    public virtual TinhTrangPhim IdtinhTrangPhimNavigation { get; set; } = null!;

    public virtual ICollection<LichChieuPhim> LichChieuPhims { get; set; } = new List<LichChieuPhim>();
}
