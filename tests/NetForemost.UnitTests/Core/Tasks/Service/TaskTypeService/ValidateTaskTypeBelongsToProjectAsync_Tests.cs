using Moq;
using NetForemost.Core.Entities.Tasks;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskTypeService;

public class ValidateTaskTypeBelongsToProjectAsync_Tests
{

    /// <summary>
    /// Check when taskTypeNotFound in the proccess
    /// </summary>
    /// <returns>error Task Type Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeNotFound_ReturnTaskTypeNotFound()
    {
        //Declarate variables
        int taskTypeId = new();
        int projectId = new();

        //Create Simulated test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, 
            out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((TaskType)null);

        //Validated Test
        var result = await taskTypeService.ValidateTaskTypeBelongsToProjectAsync(taskTypeId, projectId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeNotFound.Replace("[id]", taskTypeId.ToString()),
                       ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check when when the taskType is not from the same project
    /// </summary>
    /// <returns>error Task Type Not Owned By Project</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeIsNotFromTheSameProject_ReturnTaskTypeNotOwnedByProject()
    {
        //Declarate variables
        int taskTypeId = 1;
        int projectId = 2;
        TaskType taskType = new() { Id = 1, ProjectId = 1 };

        //Create Simulated test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, 
            out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(taskType);

        //Validated Test
        var result = await taskTypeService.ValidateTaskTypeBelongsToProjectAsync(taskTypeId, projectId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeNotOwnedByProject.Replace("[id]", taskTypeId.ToString()),
                       ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }


    /// <summary>
    /// Check when Unexpected error
    /// </summary>
    /// <returns>Test Error</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnErrorUnexpectedError()
    {
        //Declarate variables
        int taskTypeId = 1;
        int projectId = 2;
        TaskType taskType = new() { Id = 1, ProjectId = 1 };
        var ErrorMessage = "Test Error";

        //Create Simulated test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, 
            out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(ErrorMessage));

        //Validated Test
        var result = await taskTypeService.ValidateTaskTypeBelongsToProjectAsync(taskTypeId, projectId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), ErrorMessage);
    }

    /// <summary>
    /// Check when al is correct in the proccess
    /// </summary>
    /// <returns>Test Error</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declarate variables
        int taskTypeId = 1;
        int projectId = 1;
        TaskType taskType = new() { Id = 1, ProjectId = 1 };

        //Create Simulated test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, 
            out _);

        //Configure test

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(taskType);

        //Validated Test
        var result = await taskTypeService.ValidateTaskTypeBelongsToProjectAsync(taskTypeId, projectId);

        Assert.True(result.IsSuccess);

    }
}

