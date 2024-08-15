using NetForemost.Core.Dtos.Benefits;

namespace NetForemost.Core.Dtos.JobOffers;

public class JobOfferBenefitDto
{
    public int Id { get; set; }
    public BenefitDto Benefit { get; set; }
}