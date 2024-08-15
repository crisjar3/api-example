using System;
using System.Collections.Generic;

namespace NetForemost.API.Requests.JobOffers;

public class PostJobOfferRequest
{
    public bool IsActive { get; set; }
    public DateTime DateExpiration { get; set; }

    public int? CityId { get; set; }
    public int? CountryId { get; set; }
    public int CompanyId { get; set; }
    public int? ProjectId { get; set; }

    public virtual List<JobOfferBenefitRequest> Benefits { get; set; }
    public virtual List<JobOfferTalentRequest> Talents { get; set; }
}

public class JobOfferTalentRequest
{
    public string Description { get; set; }
    public string Responsibilities { get; set; }

    public int JobRoleId { get; set; }
    public int SeniorityId { get; set; }
    public int PolicyId { get; set; }
    public int ContractTypeId { get; set; }
    public int LanguageId { get; set; }
    public int LanguageLevelId { get; set; }
    public int Vacancie { get; set; }

    public List<JobOfferTalentSkillRequest> JobOfferTalentSkills { get; set; }
}

public class JobOfferTalentSkillRequest
{
    public int SkillId { get; set; }
}

public class JobOfferBenefitRequest
{
    public int BenefitId { get; set; }
}