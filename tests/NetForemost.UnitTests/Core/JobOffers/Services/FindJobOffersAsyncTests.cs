using Moq;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Specifications.JobOffers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.JobOffers.Services;

public class FindJobOffersAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Find all JobOffers
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindJobOfferAsyncIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var jobOffer = new JobOffer();

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out _, out _, out _, out _, out _, out _, out _,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out _, out _, out _, out _, out _
            );

        //Configurations for tests
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.ListAsync(
            It.IsAny<FindAllJobOffersSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<JobOffer>());

        var result = await teamService.FindJobOffersAsync();

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccur_ReturnsError()
    {
        //Delcarations of variables
        var jobOffer = new JobOffer();
        var testError = "Error to find all Job offer";

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out _, out _, out _, out _, out _, out _, out _,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out _, out _, out _, out _, out _
            );

        //Configurations for tests
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.ListAsync(
            It.IsAny<FindAllJobOffersSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await teamService.FindJobOffersAsync();

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
