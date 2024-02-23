using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class GhePhong
{
    public int Id { get; set; }

    public int Idphong { get; set; }

    public int Idghe { get; set; }

    public bool TrangThaiGhe { get; set; }

    public virtual Ghe IdgheNavigation { get; set; } = null!;

    public virtual Phong IdphongNavigation { get; set; } = null!;
}
