using Moq;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Specifications.Goals;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class GetActiveGoals_Tests
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
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _,
            out _,
            out _,
            out _,
            out _);

        //Configurated Test
        goalRepository.Setup(goalRepository => goalRepository.ListAsync(
            It.IsAny<ActiveGoalsSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<Goal>());

        //Validated Test
        var result = await goalService.GetActiveGoals(userId, 0);

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
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _,
            out _,
            out _,
            out _,
            out _);

        //Configurated Test
        goalRepository.Setup(goalRepository => goalRepository.ListAsync(
            It.IsAny<ActiveGoalsSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Validated Test
        var result = await goalService.GetActiveGoals(userId, 0);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}