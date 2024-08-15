using NetForemost.Core.Dtos.ContractTypes;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Dtos.Languages;
using NetForemost.Core.Dtos.Policies;
using NetForemost.Core.Dtos.Seniorities;

namespace NetForemost.Core.Dtos.JobOffers;

public class JobOfferTalentDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Responsibilities { get; set; }
    public int Vacancie { get; set; }

    public JobRoleDto JobRole { get; set; }
    public SeniorityDto Seniority { get; set; }
    public PolicyDto Policy { get; set; }
    public ContractTypeDto ContractType { get; set; }
    public LanguageDto Language { get; set; }
    public LanguageLevelDto LanguageLevel { get; set; }
    public List<JobOfferTalentSkillDto> JobOfferTalentSkills { get; set; }
}
