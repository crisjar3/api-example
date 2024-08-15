using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Users;

public class UserRefreshToken : BaseEntity
{
    public string Value { get; set; }
    public bool Active { get; set; }
    public bool Used { get; set; }
    public DateTime Expiration { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
}