using NetForemost.Core.Entities.Goals;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class ValidateActiveGoalBelongsToProject_Tests
{
    /// <summary>
    /// Chec when Goal Not Belong to project
    /// </summary>
    /// <returns>error Goal Not Owned By Project</returns>
    [Fact]
    public async Task WhenGoalNotBelongToProject_ReturnErrorGoalNotOwnedByProject()
    {
        //Declarate Variables
        Goal goal = new() { ProjectId = 0 };
        int projectId = 1;

        //Created Simulated Test
        var goalService = ServiceUtilities.CreateGoalService(out _, out _, out _, out _, out _, out _);
        //Configure Test

        //Validate Test

        var result = goalService.ValidateActiveGoalBelongsToProject(goal, projectId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.GoalNotOwnedByProject, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check when unexpected error
    /// </summary>
    /// <returns>unexpected error</returns>
    [Fact]
    public async Task WhenUnexpectedError_ReturnErrorr()
    {
        //Declarate Variables
        Goal goal = new() { ProjectId = 0 };
        int projectId = 1;
        var errorMessage = "Test Error";

        //Created Simulated Test
        var goalService = ServiceUtilities.CreateGoalService(out _, out _, out _, out _, out _, out _);

        //Configure Test

        //Validate Test
        var result = goalService.ValidateActiveGoalBelongsToProject(goal, projectId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }

    /// <summary>
    /// Check all is correct
    /// </summary>
    /// <returns>isSuccess</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnSuccess()
    {
        //Declarate Variables
        Goal goal = new() { ProjectId = 0 };
        int projectId = 0;

        //Created Simulated Test
        var goalService = ServiceUtilities.CreateGoalService(out _, out _, out _, out _, out _, out _);

        //Configure Test

        //Validate Test

        var result = goalService.ValidateActiveGoalBelongsToProject(goal, projectId);

        Assert.True(result.IsSuccess);
    }
}