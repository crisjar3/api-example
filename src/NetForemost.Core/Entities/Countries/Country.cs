using NetForemost.Core.Entities.JobOffers;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Countries;

public class Country : BaseEntity
{
    public string Name { get; set; }
    public string OfficialName { get; set; }
    public string CountryCode { get; set; }
    public string IsoCode { get; set; }

    public virtual ICollection<City> Cities { get; set; }
    public virtual ICollection<JobOffer> JobOffers { get; set; }
}