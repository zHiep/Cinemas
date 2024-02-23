using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class TblDisable
{
    public int Iddisable { get; set; }

    public bool Disable { get; set; }

    public DateTime LogTime { get; set; }
}
