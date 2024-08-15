using Ardalis.Specification;
using NetForemost.Core.Entities.JobOffers;

namespace NetForemost.Core.Specifications.JobOffers;
public class GetJobOfferByDateSpecification : Specification<JobOffer>
{
    /// <summary>
    /// Obtains all job offers in a date range by validating that this job offer is active and that its expiration date is greater than the specified date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public GetJobOfferByDateSpecification(DateTime startDate, DateTime endDate)
    {
        Query.Where(jobOffer => jobOffer.IsActive);

        Query.Where(jobOffer => jobOffer.DateExpiration > endDate);

        Query.Where(jobOffer => jobOffer.CreatedAt <= startDate && jobOffer.CreatedAt >= endDate);
    }
}

