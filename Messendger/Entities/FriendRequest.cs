using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class FriendRequest
{
    public int Id { get; set; }

    public string IdSender { get; set; }

    public string IdRecipient { get; set; }

    public DateOnly DateSend { get; set; }

    public virtual User IdRecipientNavigation { get; set; } = null!;

    public virtual User IdSenderNavigation { get; set; } = null!;
}
