using Moq;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Goals;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class ConfirmExtraMileGoalUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Confirm Extra Mile Goal.
    /// </summary>
    /// <returns> Return success </returns>
    [Fact]
    public async Task WhenConfirmExtraMileGoalIsCorrect_ReturnsSuccess()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";
        var goalStatusId = 1;

        var goalExtraMile = new GoalExtraMile() { Id = goalId, CreatedBy = userId };
        var user = new User() { Id = userId };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out _, out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.GetBySpecAsync(
                It.IsAny<ActiveExtraMileGoalByIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new GoalExtraMile());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        var result = await goalService.ConfirmExtraMileGoal(goalId, goalStatusId, userId);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
	/// Check if the Goal is invalid does not terminate the process correctly.
	/// </summary>
	/// <returns> Return Goal Not Found </returns>
    [Fact]
    public async Task WhenGoalIsInvalid_ReturnExtraMileGoalNotFound()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";
        var goalStatusId = 1;

        var goalExtraMile = new GoalExtraMile() { Id = goalId, CreatedBy = userId };
        var user = new User() { Id = userId };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out _, out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.GetBySpecAsync(
                null,
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new GoalExtraMile());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        var result = await goalService.ConfirmExtraMileGoal(goalId, goalStatusId, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.GoalNotFound);
    }

    /// <summary>
    /// Check if the Goal Status is invalid does not terminate the process correctly.
    /// </summary>
    /// <returns> Return Goal Status Not Found </returns>
    [Fact]
    public async Task WhenGoalStatusIdIsInvalid_ReturnGoalStatusNotFound()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";
        var goalStatusId = 1;

        var goalExtraMile = new GoalExtraMile() { Id = goalId, CreatedBy = userId };
        var user = new User() { Id = userId };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out _, out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.GetBySpecAsync(
                It.IsAny<ActiveExtraMileGoalByIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new GoalExtraMile());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync((GoalStatus)null);

        var result = await goalService.ConfirmExtraMileGoal(goalId, goalStatusId, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.GoalStatus_NotFound);
    }

    /// <summary>
	/// Verify that if an unexpected error occurs it is caught and does not break the process.
	/// </summary>
	/// <returns>Return error</returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccurToConfirmExtraMileGoal_ReturnError()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";
        var goalStatusId = 1;
        var testError = "An unexpected Error Occur to Confirm ExtraMile Goal";

        var goalExtraMile = new GoalExtraMile() { Id = goalId, CreatedBy = userId };
        var user = new User() { Id = userId };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out _, out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.GetBySpecAsync(
                It.IsAny<ActiveExtraMileGoalByIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).Throws(new Exception(testError));

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        var result = await goalService.ConfirmExtraMileGoal(goalId, goalStatusId, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
