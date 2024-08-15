using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskTypeService;

public class UpdateTaskTypeAsync_Tests
{
    /// <summary>
    /// Check when in the proccess tasktype not found
    /// </summary>
    /// <returns>Errro Task Type Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeNotFound_ReturnErrorTaskTypeNoTFound()
    {
        //Declarate variables
        TaskType taskType = new();
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((TaskType)null);

        //Validations test
        var result = await tasktypeService.UpdateTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeNotFound.Replace("[id]", taskType.Id.ToString()), ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check when in the process the user has no access to the taskType
    /// </summary>
    /// <returns>Error Company Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUserNoAccesToTaskType_ReturnErrorCompanyNoTFound()
    {
        //Declarate variables
        TaskType taskType = new();
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((TaskType)taskType);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)null);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)null);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)null);

        //Validations test
        var result = await tasktypeService.UpdateTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    ///Check when tasktype not fullfills all naming requeriments
    /// </summary>
    /// <returns>Error Task Type Name Duplicated</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUserNotFullFillsAllNamingRequeriments_ReturnErrorTaskTypeNameDuplicated()
    {
        //Declarate variables
        Project project = new() { CompanyId = 1 };
        TaskType taskType = new() { Name = "TaskType", Project = project };
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((TaskType)taskType);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Company());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new CompanyUser() { CompanyId = 1 });

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validations test
        var result = await tasktypeService.UpdateTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeNameDuplicated.Replace("[name]", taskType.Name), ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    ///Check when al proccess is correct 
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnSuccess()
    {
        //Declarate variables
        Project project = new() { CompanyId = 1 };
        TaskType taskType = new() { Name = "TaskTypeWow", Description = "Description", Project = project };
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((TaskType)taskType);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Company());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new CompanyUser() { CompanyId = 1 });

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
           It.IsAny<TaskTypesByDescriptionSpecification>(),
           It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Validations test
        var result = await tasktypeService.UpdateTaskTypeAsync(taskType, userId);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    ///Check when al proccess is correct 
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnTestError()
    {
        //Declarate variables
        Project project = new() { CompanyId = 1 };
        TaskType taskType = new() { Name = "TaskTypeWow", Description = "Description", Project = project };
        string userId = "userId";
        var errorMessage = "Test Error";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configure test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
           It.IsAny<TaskTypesByDescriptionSpecification>(),
           It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Validations test
        var result = await tasktypeService.UpdateTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}
