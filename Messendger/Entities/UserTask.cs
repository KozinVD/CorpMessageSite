using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class UserTask
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateEnd { get; set; }

    public string? Description { get; set; }

    public string IdUser { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
