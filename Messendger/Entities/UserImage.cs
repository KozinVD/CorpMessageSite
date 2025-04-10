using System;
using System.Collections.Generic;

namespace Messendger.Entities;

public partial class UserImage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
