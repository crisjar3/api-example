using Moq;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Core.Specifications.StoryPoints;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.StoryPoints.Services;

public class FindAllAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to find all Story Points
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindAllStoryPointsAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var storyPoints = new List<StoryPoint>();

        // Create the simulated service
        var storyPointService = ServiceUtilities.CreateStoryPointService(out Mock<IAsyncRepository<StoryPoint>> storyPointsRepository);

        // Configurations for tests
        storyPointsRepository.Setup(storyPointsRepository => storyPointsRepository.ListAsync(
            It.IsAny<FindAllStoryPointsSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(storyPoints);

        var result = await storyPointService.FindAllAsync();

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindAllStoryPoints_ReturnError()
    {
        // Declarations of variables
        var testError = "Unexpected Exception";

        // Create the simulated service
        var storyPointService = ServiceUtilities.CreateStoryPointService(out Mock<IAsyncRepository<StoryPoint>> storyPointsRepository);

        // Configurations for tests
        storyPointsRepository.Setup(storyPointsRepository => storyPointsRepository.ListAsync(
            It.IsAny<FindAllStoryPointsSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await storyPointService.FindAllAsync();

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
