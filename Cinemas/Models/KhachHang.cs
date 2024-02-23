using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class KhachHang
{
    public int Idkh { get; set; }

    public string TenKh { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public DateTime? NgaySinh { get; set; }

    public bool GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string Password { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool Active { get; set; }

    public string Salt { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual Role Role { get; set; } = null!;
}
