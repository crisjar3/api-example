using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Goals;
using NetForemost.Core.Specifications.ProjectCompanyUser;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class CreateExtraMileGoalTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Create Extra Mile Goal.
    /// </summary>
    /// <returns> Return success </returns>
    [Fact]
    public async Task WhenCreateExtraMileGoalIsCorrect_ReturnsSuccess()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";
        var project = new Project() { Id = 1 };
        var user = new User() { Id = "asas" };
        var company = new Company() { Id = 1 };
        var userCompany = new CompanyUser() { Id = 1, Company = company, CompanyId = company.Id, User = user, UserId = user.Id };
        var owner = new ProjectCompanyUser() { Id = 1, CompanyUser = userCompany, CompanyUserId = userCompany.Id, Project = project, ProjectId = project.Id };

        var goal = new Goal() { Id = goalId, HasExtraMileGoal = true, TargetEndDate = DateTime.MaxValue, Owner = owner, OwnerId = owner.Id };
        var goalStatus = new GoalStatus() { Id = 1 };
        var goalExtraMile = new GoalExtraMile() { Goal = new Goal() { ProjectId = 1 }, Id = goalId, CreatedBy = userId, GoalStatus = goalStatus, GoalStatusId = 1, ExtraMileTargetEndDate = DateTime.MinValue };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        goalRepository.Setup(goalRepository => goalRepository.GetBySpecAsync(
            It.IsAny<GetGoalWithRelationDataSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(new User());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.ListAsync(
                It.IsAny<ExtraMileGoalsByGoalIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new List<GoalExtraMile>());

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.AnyAsync(
            It.IsAny<CheckUserExistInProjectSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await goalService.CreateExtraMileGoal(goalExtraMile, userId, -6);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
	/// Check if the Goal not exist and does not terminate the process correctly.
	/// </summary>
	/// <returns> Return Goal Not Found </returns>
    [Fact]
    public async Task WhenGoalNotFound_ReturnGoalNotFound()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";

        var goal = new Goal() { Id = goalId, HasExtraMileGoal = true };
        var goalStatus = new GoalStatus() { Id = 1 };
        var goalExtraMile = new GoalExtraMile() { Id = 2, CreatedBy = userId, GoalStatus = goalStatus, GoalStatusId = goalStatus.Id };

        var invalidGoalID = 2;

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            invalidGoalID,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Goal());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.ListAsync(
                It.IsAny<ExtraMileGoalsByGoalIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new List<GoalExtraMile>());

        var result = await goalService.CreateExtraMileGoal(goalExtraMile, userId, -6);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.GoalNotFound.Replace("[id]", goalExtraMile.GoalId.ToString()));
    }

    /// <summary>
    /// Check if the Goal is marked as completed and does not terminate the process correctly.
    /// </summary>
    /// <returns> Return Goal Completed Already </returns>
    [Fact]
    public async Task WhenGoalIsMarkedCompleted_ReturnGoalCompletedAlredy()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";

        var goal = new Goal() { Id = goalId, HasExtraMileGoal = true, ActualEndDate = DateTime.UtcNow.AddDays(-1) };
        var goalStatus = new GoalStatus() { Id = 1 };
        var goalExtraMile = new GoalExtraMile() { Id = 2, CreatedBy = userId, Goal = goal, GoalId = goal.Id, GoalStatus = goalStatus, GoalStatusId = goalStatus.Id };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.ListAsync(
                It.IsAny<ExtraMileGoalsByGoalIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new List<GoalExtraMile>());

        var result = await goalService.CreateExtraMileGoal(goalExtraMile, userId, -6);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Goal_CompletedAlready_Error);
    }

    /// <summary>
    /// Check if the target date is not less than initial goal date and does not terminate the process correctly.
    /// </summary>
    /// <returns> Return end date less than start date </returns>
    [Fact]
    public async Task WhenTargetDateIsNotLessThanInitialGoalDate_ReturnInvalidGoal()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";

        var goalStatus = new GoalStatus() { Id = 1 };
        var goal = new Goal() { Id = goalId, HasExtraMileGoal = true, TargetEndDate = DateTime.UtcNow };
        var goalExtraMile = new GoalExtraMile() { Id = goalId, CreatedBy = userId, ExtraMileTargetEndDate = DateTime.UtcNow.AddDays(1), GoalStatus = goalStatus, GoalStatusId = goalStatus.Id };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Goal());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.ListAsync(
                It.IsAny<ExtraMileGoalsByGoalIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new List<GoalExtraMile>());

        var result = await goalService.CreateExtraMileGoal(goalExtraMile, userId, -6);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.InvalidGoal_EndDateLessThanStarDate);
    }

    /// <summary>
    /// Check if the Goal Status is invalid does not terminate the process correctly.
    /// </summary>
    /// <returns> Returns Goal Status Not Found </returns>
    [Fact]
    public async Task WhenGoalStatusIdIsInvalid_ReturnGoalStatusIdNotFound()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";

        var goal = new Goal() { Id = goalId, HasExtraMileGoal = true, EstimatedHours = 1 };
        var goalStatus = new GoalStatus() { Id = 1 };
        var goalExtraMile = new GoalExtraMile() { Id = goalId, CreatedBy = userId, GoalStatus = goalStatus, GoalStatusId = 1 };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Goal());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((GoalStatus)null);

        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.ListAsync(
                It.IsAny<ExtraMileGoalsByGoalIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new List<GoalExtraMile>());

        var result = await goalService.CreateExtraMileGoal(goalExtraMile, userId, -6);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.GoalStatus_NotFound);
    }

    /// <summary>
	/// Verify that if an unexpected error occurs it is caught and does not break the process.
	/// </summary>
	/// <returns>Return error</returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToCreateExtraMileGoal_ReturnError()
    {
        //Delcarations of variables
        var goalId = 1;
        var userId = "1";
        var testError = "An unexpected Errors Occur to Create an ExtraMile Goal";

        var goal = new Goal() { Id = goalId, HasExtraMileGoal = true };
        var goalStatus = new GoalStatus() { Id = 1 };
        var goalExtraMile = new GoalExtraMile() { Id = goalId, CreatedBy = userId, GoalStatus = goalStatus, GoalStatusId = goalStatus.Id };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
            out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        extraMileGoalRepository.Setup(extraMileGoalRepository => extraMileGoalRepository.ListAsync(
                It.IsAny<ExtraMileGoalsByGoalIdSpecification>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(new List<GoalExtraMile>());

        var result = await goalService.CreateExtraMileGoal(goalExtraMile, userId, -6);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
