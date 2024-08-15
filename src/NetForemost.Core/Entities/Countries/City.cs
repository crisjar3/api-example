using NetForemost.Core.Entities.JobOffers;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Countries;

public class City : BaseEntity
{
    public string Name { get; set; }
    public string IsoCode { get; set; }
    public int CountryId { get; set; }

    public virtual Country Country { get; set; }
    public virtual ICollection<JobOffer> JobOffers { get; set; }
}