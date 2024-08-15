using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobOffers;

public class JobOfferTalent : BaseEntity
{
    public string Description { get; set; }
    public string Responsibilities { get; set; }

    public int JobOfferId { get; set; }
    public int JobRoleId { get; set; }
    public int SeniorityId { get; set; }
    public int PolicyId { get; set; }
    public int ContractTypeId { get; set; }
    public int LanguageId { get; set; }
    public int LanguageLevelId { get; set; }
    public int Vacancie { get; set; }

    public virtual JobOffer JobOffer { get; set; }
    public virtual JobRole JobRole { get; set; }
    public virtual Seniority Seniority { get; set; }
    public virtual Policy Policy { get; set; }
    public virtual ContractType ContractType { get; set; }
    public virtual Language Language { get; set; }
    public virtual LanguageLevel LanguageLevel { get; set; }
    public virtual ICollection<JobOfferTalentSkill> JobOfferTalentSkills { get; set; }
}