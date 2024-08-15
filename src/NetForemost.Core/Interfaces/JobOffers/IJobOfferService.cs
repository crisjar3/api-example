using Ardalis.Result;
using NetForemost.Core.Entities.JobOffers;

namespace NetForemost.Core.Interfaces.JobOffers;

public interface IJobOfferService
{
    /// <summary>
    /// Create a new Teammate request for a company.
    /// </summary>
    /// <param name="jobOffer">The request to create</param>
    /// <param name="userId">The user who creates it</param>
    /// <returns>Returns the request created with all the correct values.</returns>
    Task<Result<JobOffer>> CreateJobOfferAsync(JobOffer jobOffer, string userId);

    /// <summary>
    /// Change the status of a job offer.
    /// </summary>
    /// <param name="jobOfferId">The job offer Id.</param>
    Task<Result<JobOffer>> UpdateJobOfferStatus(int jobOfferId);

    /// <summary>
    /// Find all JobOffer published
    /// </summary>
    Task<Result<List<JobOffer>>> FindJobOffersAsync();
}
