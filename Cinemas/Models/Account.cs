using System;
using System.Collections.Generic;

namespace Cinemas.Models;

public partial class Account
{
    public int Idaccount { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string? Avatar { get; set; }

    public bool Active { get; set; }

    public virtual Role Role { get; set; } = null!;
}
