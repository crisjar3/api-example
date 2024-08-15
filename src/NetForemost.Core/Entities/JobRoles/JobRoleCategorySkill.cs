using NetForemost.Core.Entities.Skills;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobRoles;
public class JobRoleCategorySkill : BaseEntity
{
    public int SkillId { get; set; }
    public int JobRoleCategoryId { get; set; }

    public virtual Skill Skill { get; set; }
    public virtual JobRoleCategory JobRoleCategory { get; set; }
}
