using Moq;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Specifications.Goals;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class FindAllGoalsAsyncTests
{

    /// <summary>
    /// Verify the correct functioning of the entire process to Find All Extra Mile Goals.
    /// </summary>
    /// <returns> Return success </returns>
    [Fact]
    public async Task WhenFindAllExtraMileGoalsIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var userId = "user_id";
        var description = "Is a description";
        var projectId = 1;
        var storyPoints = 3;
        var dateStartTo = DateTime.Now;
        var dateStartFrom = DateTime.Now;
        var actualendDateTo = DateTime.Now;
        var actualendDateFrom = DateTime.Now;
        var scrumMasterId = "scrum_id";
        var jiraTicketId = "NFCA-Test";
        var priorityLevel = "High";
        var goalStatusId = 1;
        var pageNumber = 3;
        var perPage = 1;
        var companyId = 1;
        var estimatedHours = 1;

        var numberEntitiesFind = 2;

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.CountAsync(
            It.IsAny<FindAllGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(numberEntitiesFind);

        goalRepository.Setup(goalRepository => goalRepository.ListAsync(
            It.IsAny<FindAllGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<FindAllGoalsDto>());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        var result = await goalService.FindAllGoalsAsync(
            userId, description, estimatedHours, projectId, storyPoints, dateStartTo, dateStartFrom,
            actualendDateTo, actualendDateFrom, actualendDateTo, actualendDateFrom, scrumMasterId, jiraTicketId, priorityLevel, -6, goalStatusId, companyId,
            pageNumber, perPage
            );

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
	/// Verify that if an unexpected error occurs it is caught and does not break the process.
	/// </summary>
	/// <returns>Return error</returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccurs_ReturnError()
    {
        //Delcarations of variables
        var userId = "user_id";
        var description = "Is a description";
        var projectId = 1;
        var storyPoints = 3;
        var dateStartTo = DateTime.Now;
        var dateStartFrom = DateTime.Now;
        var actualendDateTo = DateTime.Now;
        var actualendDateFrom = DateTime.Now;
        var scrumMasterId = "scrum_id";
        var jiraTicketId = "NFCA-Test";
        var priorityLevel = "High";
        var goalStatusId = 1;
        var pageNumber = 3;
        var perPage = 1;
        var testError = "A exception occur to find all Goals";
        var companyId = 1;
        var estimatedHours = 1;

        //Create the simulated service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out _, out _, out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository, out _, out _
            );

        //Configurations for tests
        goalRepository.Setup(goalRepository => goalRepository.CountAsync(
            It.IsAny<FindAllGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        goalRepository.Setup(goalRepository => goalRepository.ListAsync(
            It.IsAny<FindAllGoalsSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<FindAllGoalsDto>());

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new GoalStatus());

        var result = await goalService.FindAllGoalsAsync(
            userId, description, estimatedHours, projectId, storyPoints, dateStartTo, dateStartFrom,
            actualendDateTo, actualendDateFrom, actualendDateTo, actualendDateFrom, scrumMasterId, jiraTicketId, priorityLevel, -6, goalStatusId, companyId,
            pageNumber, perPage
            );

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
