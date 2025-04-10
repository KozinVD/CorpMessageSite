using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class ChatParticipant
{
    public int Id { get; set; }

    public string IdUser { get; set; }

    public int IdChat { get; set; }

    public virtual Chat IdChatNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
