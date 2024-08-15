using Moq;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Specifications.Goals;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class GetActiveExtraMileGoals_Tests
{
    /// <summary>
    /// When al is correct in the proccess
    /// </summary>
    /// <returns>Is Success</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declarate variables
        string userId = "UserId";

        //Create simulated test
        var goalService = ServiceUtilities.CreateGoalService(
            out _,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMileRepository,
            out _,
            out _,
            out _,
            out _);

        //Configurated Test
        goalExtraMileRepository.Setup(goalExtraMileRepository => goalExtraMileRepository.ListAsync(
            It.IsAny<ActiveExtraMileGoalsSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<GoalExtraMile>());

        //Validated Test
        var result = await goalService.GetActiveExtraMileGoals(userId, -6);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// When in a proccess hace unexpected Error
    /// </summary>
    /// <returns>Test Error</returns>
    [Fact]
    public async Task WhenHaveUnexpectedError_ReturnUnexpectedError()
    {
        //Declarate variables
        string userId = "UserId";
        var errorMessage = "Test Error";

        //Create simulated test
        var goalService = ServiceUtilities.CreateGoalService(
            out _,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMileRepository,
            out _,
            out _,
            out _,
            out _);

        //Configurated Test
        goalExtraMileRepository.Setup(goalExtraMileRepository => goalExtraMileRepository.ListAsync(
            It.IsAny<ActiveExtraMileGoalsSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Validated Test
        var result = await goalService.GetActiveExtraMileGoals(userId, -6);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}