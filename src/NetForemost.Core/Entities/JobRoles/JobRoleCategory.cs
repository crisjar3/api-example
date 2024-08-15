using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobRoles;

public class JobRoleCategory : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDefault { get; set; }
    public int? CompanyId { get; set; }

    public virtual ICollection<JobRole> JobRoles { get; set; }
    public virtual ICollection<JobRoleCategorySkill> JobRoleCategorySkills { get; set; }
    public virtual ICollection<JobRoleCategoryTranslation> JobRoleCategoryTranslations { get; set; }
    public virtual Company? Company { get; set; }
}