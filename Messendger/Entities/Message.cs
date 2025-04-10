using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class Message
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public string IdUser { get; set; }

    public int IdChat { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual Chat IdChatNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
