using Moq;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Specifications.Goals;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class GetActiveGoalsSummaryTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Find Get active goals summary.
    /// </summary>
    /// <returns> Return success </returns>
    [Fact]
    public async Task WhenGetActiveGoalsSummaryIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var userId = "user_id";

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMileRepository, out _,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.ListAsync(
            It.IsAny<ActiveGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<Goal>());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        goalExtraMileRepository.Setup(goalExtraMileRepository => goalExtraMileRepository.ListAsync(
            It.IsAny<ActiveExtraMileGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<GoalExtraMile>());

        var result = await goalService.GetActiveGoalsSummary(userId, 0);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
	/// Verify that if an unexpected error occurs it is caught and does not break the process.
	/// </summary>
	/// <returns>Return error</returns>
    [Fact]
    public async Task WhenAnUnexpectedExceptionOccurs_ReturnError()
    {
        //Delcarations of variables
        var userId = "user_id";
        var testError = "Object reference not set to an instance of an object.";

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMileRepository, out _,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.ListAsync(
            It.IsAny<ActiveGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        goalExtraMileRepository.Setup(goalExtraMileRepository => goalExtraMileRepository.ListAsync(
            It.IsAny<ActiveExtraMileGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<GoalExtraMile>());

        var result = await goalService.GetActiveGoalsSummary(userId, 0);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
