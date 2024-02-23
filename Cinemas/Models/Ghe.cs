using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class Ghe
{
    public int Idghe { get; set; }

    public string TenGhe { get; set; } = null!;

    public virtual ICollection<GhePhong> GhePhongs { get; set; } = new List<GhePhong>();
}
