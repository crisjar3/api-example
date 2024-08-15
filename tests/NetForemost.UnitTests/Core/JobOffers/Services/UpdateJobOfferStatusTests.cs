using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.JobOffers.Services;

public class UpdateJobOffesrStatusTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Update Offer Status
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenUpdateOfferStatus_ReturnSuccess()
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
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.UpdateAsync(
            It.IsAny<JobOffer>(),
            It.IsAny<CancellationToken>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await teamService.UpdateJobOfferStatus(jobOffer.Id);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Job offer is'nt valid and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Job Offer not exist </returns>
    [Fact]
    public async Task WhenJobOfferNotExist_ReturnJobOfferNotExist()
    {
        //Delcarations of variables
        var jobOffer = new JobOffer();
        var idNotValid = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out _, out _, out _, out _, out _, out _, out _,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out _, out _, out _, out _, out _
            );

        //Configurations for tests
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            idNotValid,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.UpdateAsync(
            It.IsAny<JobOffer>(),
            It.IsAny<CancellationToken>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await teamService.UpdateJobOfferStatus(jobOffer.Id);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobOfferNotExist);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccur_ReturnError()
    {
        //Delcarations of variables
        var testError = "Error to update Job offer status";
        var jobOffer = new JobOffer();
        var jobOfferId = 1;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out _, out _, out _, out _, out _, out _, out _,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out _, out _, out _, out _, out _
            );

        //Configurations for tests
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await teamService.UpdateJobOfferStatus(jobOfferId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}