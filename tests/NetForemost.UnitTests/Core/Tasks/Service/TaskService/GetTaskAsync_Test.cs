using Moq;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskService;

public class GetTaskAsync_Test
{
    /// <summary>
    /// Verify if the proccess is correct
    /// </summary>
    /// <returns>
    /// IsSuccess
    /// </returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declarate variables
        string userId = "";
        string description = "";
        int typeId = new();
        int goalId = new();
        int projectId = new();
        int companyId = new();
        DateTime? targetEndDateFrom = new();
        DateTime? targetEndDateTo = new();
        int pageNumber = new();
        int perPage = new();
        List<GetTasksQueryableDto> tasks = new();

        //Create simulated service
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out _,
            out _,
            out _,
            out _
            );

        //Configuration for test
        taskRepository.Setup(taskRepository => taskRepository.CountAsync(
            It.IsAny<TasksQueryableSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(10);

        taskRepository.Setup(taskRepository => taskRepository.ListAsync(
            It.IsAny<TasksQueryableSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(tasks);

        var result = await taskService.GetTasksAsync(userId, description, typeId, goalId, projectId,
                                                companyId, targetEndDateFrom, targetEndDateTo, pageNumber, perPage);

        //validation for test
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// check the proccess when unexpected error
    /// </summary>
    /// <returns>
    /// Test Error
    /// </returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnErrorTestError()
    {
        //Declarate variables
        string userId = "";
        string description = "";
        int typeId = new();
        int goalId = new();
        int projectId = new();
        int companyId = new();
        DateTime? targetEndDateFrom = new();
        DateTime? targetEndDateTo = new();
        int pageNumber = new();
        int perPage = new();
        List<NetForemost.Core.Entities.Tasks.Task> tasks = new();
        var messageError = "Test Error";

        //Create simulated service
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out _,
            out _,
            out _,
            out _
            );

        //Configuration for test
        taskRepository.Setup(taskRepository => taskRepository.CountAsync(
            It.IsAny<TasksQueryableSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(messageError));

        taskRepository.Setup(taskRepository => taskRepository.ListAsync(
            It.IsAny<TasksQueryableSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(messageError));

        var result = await taskService.GetTasksAsync(userId, description, typeId, goalId, projectId,
                                                companyId, targetEndDateFrom, targetEndDateTo, pageNumber, perPage);

        //validation for test
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), messageError);
    }
}
