using Moq;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Specifications.ProjectCompanyUser;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class ValidateActiveGoalBelongToUser_Tests
{
    /// <summary>
    /// When all is correct in the proccess
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declarate variables
        var userId = 1;
        var owner = new ProjectCompanyUser() { Id = 1, Project = new Project() { Id = 1 }, ProjectId = 1 };
        int goalId = new();
        Goal goal = new() { OwnerId = owner.Id, ActualEndDate = null, Owner = owner };

        //Create simulated test
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out _, out _,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository);

        //Configurated Test
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.AnyAsync(
            It.IsAny<CheckUserExistInProjectSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        //Validated Test
        var result = await goalService.ValidateActiveGoalBelongsToUser(goalId, userId.ToString());

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// When unexpectedError in the proccess
    /// </summary>
    /// <returns>Unexpected Error</returns>
    [Fact]
    public async Task WhenUnexpectedError_ReturnUnexpectedError()
    {
        //Declarate variables
        var userId = 1;
        int goalId = new();
        Goal goal = new() { OwnerId = userId, ActualEndDate = DateTime.Parse("12-12-9999") };
        var errorMessage = "Test Error";

        //Create simulated test
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out _, out _, out _);

        //Configurated Test
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Validated Test
        var result = await goalService.ValidateActiveGoalBelongsToUser(goalId, userId.ToString());

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }

    /// <summary>
    /// When in the proccess goal not exist
    /// </summary>
    /// <returns>Error Goal Not Found</returns>
    [Fact]
    public async Task WhenGoalNotExist_ReturnErrorGoalNotFound()
    {
        //Declarate variables
        var userId = 1;
        int goalId = new();
        Goal goal = new() { OwnerId = userId, ActualEndDate = DateTime.Parse("12-12-9999") };

        //Create simulated test
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out _, out _, out _);

        //Configurated Test
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Goal)null);

        //Validated Test
        var result = await goalService.ValidateActiveGoalBelongsToUser(goalId, userId.ToString());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.GoalNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// When in the proccess goal not belong to user
    /// </summary>
    /// <returns>Error Goal Not Owned By User</returns>
    [Fact]
    public async Task WhenGoalNotBelongToUser_ReturnErrorGoal_NotOwnedByUser()
    {
        //Declarate variables
        string userId = "UserId";
        int goalId = new();
        Goal goal = new() { OwnerId = 1, ActualEndDate = DateTime.Parse("12-12-9999") };

        //Create simulated test
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out _, out _, out _);

        //Configurated Test
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(goal);

        //Validated Test
        var result = await goalService.ValidateActiveGoalBelongsToUser(goalId, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.Goal_NotOwnedByUser, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// When in the proccess ActualDate of Goal is null
    /// </summary>
    /// <returns>Error Goal Not Active</returns>
    [Fact]
    public async Task WhenEndDateOfGoalNotFound_ReturnErrorGoal_NotOwnedByUser()
    {
        //Declarate variables
        var ownerId = 1;
        int goalId = new();
        Goal goal = new() { OwnerId = ownerId, ActualEndDate = DateTime.Now };

        //Create simulated test
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out _, out _,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository);

        //Configurated Test
        goalRepository.Setup(goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.AnyAsync(
            It.IsAny<CheckUserExistInProjectSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        //Validated Test
        var result = await goalService.ValidateActiveGoalBelongsToUser(goalId, ownerId.ToString());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.GoalNotActive, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }
}
