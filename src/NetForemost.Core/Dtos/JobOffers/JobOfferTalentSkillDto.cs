using NetForemost.Core.Dtos.Skills;

namespace NetForemost.Core.Dtos.JobOffers;

public class JobOfferTalentSkillDto
{
    public int Id { get; set; }

    public int SkillId { get; set; }
    public SkillDto Skill { get; set; }
}