using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using NetForemost.Core.Entities.JobOffers;

namespace NetForemost.Core.Specifications.JobOffers;

/// <summary>
/// Obtains all job offers validating that this job offer is active
/// </summary>
/// <param name="includeRelations">If query include relations.</param>

public class FindAllJobOffersSpecification : Specification<JobOffer>
{
    public FindAllJobOffersSpecification(bool includeRelations = false)
    {
        Query.Where(jobOffer => jobOffer.IsActive);

        if (includeRelations)
        {
            Query.Include(jobOffer => jobOffer.Project);
            Query.Include(jobOffer => jobOffer.Company);
            Query.Include(jobOffer => jobOffer.Country);
            Query.Include(jobOffer => jobOffer.City);
            Query.Include(jobOffer => jobOffer.Benefits);
            Query.Include(jobOffer => jobOffer.Talents);
        }
    }
}