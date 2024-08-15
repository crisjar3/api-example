using Moq;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskTypeService;

public class GetTaskTypeAsync_Tests
{
    /// <summary>
    /// Check when all procceds is correct
    /// </summary>
    /// <returns>IsSuccess</returns>
    /// 
    [Fact]
    public async System.Threading.Tasks.Task WhenAllProccessIsCorrect_ReturnSuccess()
    {
        //Declaration Variables
        string userId = "UserId";
        string name = "Name";
        string description = "Description";
        int projectId = 1;
        int companyId = 1;
        int pageNumber = 10;
        int perPage = 1;

        //Create simulated test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
            out _
            );

        //Configuration Test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.CountAsync(
            It.IsAny<CancellationToken>())).ReturnsAsync(10);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.ListAsync(
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<TaskType>() { new(), new(), new() });
        //Validations Test
        var result = await taskTypeService.GetTaskTypesAsync(userId, name, description, projectId, companyId,
                                                            pageNumber, perPage);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Check when in proccess is unexpected error
    /// </summary>
    /// <returns>UnexpectedError</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnError()
    {
        //DeclarationVariables
        string userId = "UserId";
        string name = "Name";
        string description = "Description";
        int projectId = 1;
        int companyId = 1;
        int pageNumber = 10;
        int perPage = 1;
        var messageError = "Test Error";

        //Create simulated test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
             out _,
             out _,
             out _,
             out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
             out _
             );

        //Configuration Test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.CountAsync(
            It.IsAny<TaskTypesQueryableSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(messageError));

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.ListAsync(
            It.IsAny<TaskTypesQueryableSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(messageError));

        //Validations Test
        var result = await taskTypeService.GetTaskTypesAsync(userId, name, description, projectId, companyId,
                                                            pageNumber, perPage);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), messageError);
    }
}
