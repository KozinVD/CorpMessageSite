using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class Chat
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsGroup { get; set; }

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
