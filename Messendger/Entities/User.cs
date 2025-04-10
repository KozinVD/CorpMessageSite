using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class User : IdentityUser
{

    public DateOnly LastLoginDate { get; set; }

    public int? IdPhoto { get; set; }

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual ICollection<Friend> FriendIdFriendNavigations { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendIdUserNavigations { get; set; } = new List<Friend>();

    public virtual ICollection<FriendRequest> FriendRequestIdRecipientNavigations { get; set; } = new List<FriendRequest>();

    public virtual ICollection<FriendRequest> FriendRequestIdSenderNavigations { get; set; } = new List<FriendRequest>();

    public virtual UserImage? IdPhotoNavigation { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual UserInfo? UserInfo { get; set; }
}
