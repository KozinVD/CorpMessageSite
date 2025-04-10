using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class File
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IdMessage { get; set; }

    public virtual Message IdMessageNavigation { get; set; } = null!;
}
