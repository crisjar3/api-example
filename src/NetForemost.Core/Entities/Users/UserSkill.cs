using NetForemost.Core.Entities.Skills;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Users;

public class UserSkill : BaseEntity
{
    public int SkillId { get; set; }
    public string UserId { get; set; }

    public virtual Skill Skill { get; set; }
    public virtual User User { get; set; }
}