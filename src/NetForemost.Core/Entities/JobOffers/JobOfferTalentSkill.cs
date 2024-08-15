using NetForemost.Core.Entities.Skills;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobOffers;

public class JobOfferTalentSkill : BaseEntity
{
    public int JobOfferTalentId { get; set; }
    public int SkillId { get; set; }

    public virtual Skill Skill { get; set; }
    public virtual JobOfferTalent JobOfferTalent { get; set; }
}
