using NetForemost.Core.Entities.JobOffers;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Seniorities;

public class Seniority : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<JobOfferTalent> JobOfferJobRoles { get; set; }
}