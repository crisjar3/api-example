using Moq;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Seniorities.Services;

public class FindSenioritiesAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to find all Seniorities
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindAllSenioritiesAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var seniorities = new List<Seniority>();

        // Create the simulated service
        var priorityLevelService = ServiceUtilities.CreateSeniorityService(out Mock<IAsyncRepository<Seniority>> seniorityRepository);

        // Configurations for tests
        seniorityRepository.Setup(seniorityRepository => seniorityRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniorities);

        var result = await priorityLevelService.FindSenioritiesAsync();

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindAllSeniorities_ReturnError()
    {
        // Declarations of variables
        var testError = "Error to find all Seniorities";

        // Create the simulated service
        var priorityLevelService = ServiceUtilities.CreateSeniorityService(out Mock<IAsyncRepository<Seniority>> seniorityRepository);

        // Configurations for tests
        seniorityRepository.Setup(seniorityRepository => seniorityRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await priorityLevelService.FindSenioritiesAsync();

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
