using Moq;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskTypeService;

public class ValidateNamingTaskTypeAsync_Tests
{
    /// <summary>
    /// Check when in proccess task type Name duplicated
    /// </summary>
    /// <returns>Error Task Type Name Duplicated</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeNameDuplicated_ReturnErrorTakTypeNameDuplicated()
    {
        //Declaration variables
        TaskType taskType = new() { Name = "Name" };

        //Create Simulated Test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, 
            out _);

        //Configure Test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validation Test
        var result = await taskTypeService.ValidateNamingTaskTypeAsync(taskType);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeNameDuplicated.Replace("[name]", taskType.Name),
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check when in proccess task type description duplicated
    /// </summary>
    /// <returns> Error Task Type Description Duplicated</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeDescriptionDuplicated_ReturnErrorTakTypeDescriptionDuplicated()
    {
        //Declaration variables
        TaskType taskType = new() { Name = "Name", Description = "Description" };

        //Create Simulated Test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, 
            out _);

        //Configure Test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByDescriptionSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validation Test
        var result = await taskTypeService.ValidateNamingTaskTypeAsync(taskType);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeDescriptionDuplicated.Replace("[description]", taskType.Description),
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// When in the proccess Unexpected Error
    /// </summary>
    /// <returns>TestError</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnUnexpectedError()
    {
        //Declaration variables
        TaskType taskType = new() { Name = "Name", Description = "Description" };
        var errorMessage = "Test Error";

        //Create Simulated Test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, 
            out _);

        //Configure Test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByDescriptionSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Validation Test
        var result = await taskTypeService.ValidateNamingTaskTypeAsync(taskType);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }

    /// <summary>
    /// When al is correct in the proccess
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration variables
        TaskType taskType = new() { Name = "Name", Description = "Description" };

        //Create Simulated Test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
            out _);
        //Configure Test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByDescriptionSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Validation Test
        var result = await taskTypeService.ValidateNamingTaskTypeAsync(taskType);

        Assert.True(result.IsSuccess);
    }
}
