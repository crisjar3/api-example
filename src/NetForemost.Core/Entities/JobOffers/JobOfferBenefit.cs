using NetForemost.Core.Entities.Benefits;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobOffers;

public class JobOfferBenefit : BaseEntity
{
    public int JobOfferId { get; set; }
    public int BenefitId { get; set; }

    public virtual JobOffer JobOffer { get; set; }
    public virtual Benefit Benefit { get; set; }
}