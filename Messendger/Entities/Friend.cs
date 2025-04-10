using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class Friend
{
    public int Id { get; set; }

    public string IdUser { get; set; }

    public string IdFriend { get; set; }

    public virtual User IdFriendNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
