using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class UserInfo
{
    public string Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Lastname { get; set; }

    public DateOnly Birthday { get; set; }

    public int IdJob { get; set; }

    public virtual Job IdJobNavigation { get; set; } = null!;

    public virtual User IdNavigation { get; set; } = null!;
}
