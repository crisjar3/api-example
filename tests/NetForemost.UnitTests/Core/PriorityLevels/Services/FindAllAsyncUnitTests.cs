using Moq;
using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Specifications.PriorityLevels;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.PriorityLevels.Services;

public class FindAllAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to find all PriorityLevels
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindAllPriorityLevelsAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var priorityLevels = new List<PriorityLevel>();

        // Create the simulated service
        var priorityLevelService = ServiceUtilities.CreatePriorityLevelService(out Mock<IAsyncRepository<PriorityLevel>> priorityLevelRepository);

        // Configurations for tests
        priorityLevelRepository.Setup(priorityLevelRepository => priorityLevelRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(priorityLevels);

        var result = await priorityLevelService.FindAllAsync(1);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindAllPriorityLevels_ReturnError()
    {
        // Declarations of variables
        var testError = "Error to find all PriorityLevels";
        int languageId = 10;
        var priorityLevelTranslationRepository = new Mock<IAsyncRepository<PriorityLevelTranslation>>();
        // Create the simulated service
        var priorityLevelService = ServiceUtilities.CreatePriorityLevelService(out _, priorityLevelTranslationRepository);

        // Configurations for tests
        priorityLevelTranslationRepository.Setup(priorityLevelTranslationRepository => priorityLevelTranslationRepository.ListAsync(
            It.IsAny<PriorityLevelTranslationSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(() =>
            {
                throw new Exception(testError);
            });

        var result = await priorityLevelService.FindAllAsync(languageId);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
