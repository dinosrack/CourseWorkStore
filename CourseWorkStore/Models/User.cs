using System;
using System.Collections.Generic;

namespace CourseWorkStore.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Pass { get; set; } = null!;
}
