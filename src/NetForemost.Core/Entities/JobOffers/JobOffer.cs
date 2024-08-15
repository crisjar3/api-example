using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Projects;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobOffers;

public class JobOffer : BaseEntity
{
    public bool IsActive { get; set; }
    public DateTime DateExpiration { get; set; }

    public int? CityId { get; set; }
    public int? CountryId { get; set; }
    public int CompanyId { get; set; }
    public int? ProjectId { get; set; }

    public virtual City? City { get; set; }
    public virtual Country? Country { get; set; }
    public virtual Company Company { get; set; }
    public virtual Project? Project { get; set; }

    public virtual ICollection<JobOfferBenefit> Benefits { get; set; }
    public virtual ICollection<JobOfferTalent> Talents { get; set; }
}