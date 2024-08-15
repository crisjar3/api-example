using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Roles;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Languages;

public class Language : BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<RoleTranslation> RoleTranslations { get; set; }
    public virtual ICollection<JobRoleTranslation> WorkRoleTranslations { get; set; }
    public virtual ICollection<JobRoleCategoryTranslation> WorkRoleCategoryTranslations { get; set; }
}