using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.ProjectCompanyUser;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class ConfirmGoalTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Confirm a Goal
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenConfirmrGoalIsValid_ReturnSuccess()
    {
        //Delcarations of variables
        var goalId = 1;
        var goalStatusId = 1;

        var project = new Project() { Id = 1 };
        var owner = new ProjectCompanyUser() { Id = 1, ProjectId = project.Id, Project = project };
        var goal = new Goal() { Id = goalId, OwnerId = owner.Id, Owner = owner };
        var user = new User() { Id = owner.Id.ToString() };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<CheckUserExistInProjectSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await goalService.ConfirmGoal(goalId, goalStatusId, owner.Id.ToString());

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if a Goal not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Goal Not Found </returns>
    [Fact]
    public async Task WhenGoalNotExist_ReturnGoalNotFound()
    {
        //Delcarations of variables
        var goalId = 1;
        var goalIdInvalid = 2;
        var goalStatusId = 1;
        var ownerId = 2;
        var userId = "1";

        var goal = new Goal() { Id = goalId, OwnerId = ownerId };
        var user = new User() { Id = userId };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            goalIdInvalid,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Goal());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await goalService.ConfirmGoal(goalId, goalStatusId, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.GoalNotFound);
    }

    /// <summary>
    /// It checks if a Goal not belong to the user and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Goal Not Belong to the User </returns>
    [Fact]
    public async Task WhenUserIdIsDistinctToOwnerId_ReturnGoalNotBelongToTheUser()
    {
        //Delcarations of variables
        var goalId = 1;
        var goalStatusId = 1;
        var userIdInvalid = "2";
        var userId = "1";

        var project = new Project() { Id = 1 };
        var owner = new ProjectCompanyUser() { Id = 2, ProjectId = project.Id, Project = project };
        var goal = new Goal() { Id = goalId, OwnerId = owner.Id, Owner = owner };
        var user = new User() { Id = userId };

        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<CheckUserExistInProjectSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        userManager.Setup(userManager => userManager.FindByIdAsync(
            userIdInvalid
            )).ReturnsAsync(new User());

        var result = await goalService.ConfirmGoal(goalId, goalStatusId, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Goal_NotOwnedByUser);
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
        var userId = 1;
        var goalStatusId = 1;

        var project = new Project() { Id = 1 };
        var owner = new ProjectCompanyUser() { Id = 2, ProjectId = project.Id, Project = project };
        var goal = new Goal() { Id = goalId, OwnerId = owner.Id, Owner = owner };
        var user = new User() { Id = userId.ToString() };

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<CheckUserExistInProjectSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((GoalStatus)null);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await goalService.ConfirmGoal(goalId, goalStatusId, userId.ToString());

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.GoalStatus_NotFound);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccursToConfirmGoal_ReturnError()
    {
        //Delcarations of variables
        var goalId = 1;
        var goalStatusId = 1;
        var userId = 1;

        var goal = new Goal() { Id = goalId, OwnerId = userId };
        var user = new User() { Id = userId.ToString() };
        var testError = "Error to Confirm Goal";

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager, out _
            );

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await goalService.ConfirmGoal(goalId, goalStatusId, userId.ToString());

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}