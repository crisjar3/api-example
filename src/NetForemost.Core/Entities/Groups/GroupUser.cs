using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Groups;

public class GroupUser : BaseEntity
{
    public int GroupId { get; set; }
    public string UserId { get; set; }

    public virtual Group Group { get; set; }
    public virtual User User { get; set; }
}