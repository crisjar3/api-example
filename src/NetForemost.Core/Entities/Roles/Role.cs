using Microsoft.AspNetCore.Identity;

namespace NetForemost.Core.Entities.Roles;

public class Role : IdentityRole<string>
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public virtual ICollection<RoleTranslation> RoleTranslations { get; set; }
}