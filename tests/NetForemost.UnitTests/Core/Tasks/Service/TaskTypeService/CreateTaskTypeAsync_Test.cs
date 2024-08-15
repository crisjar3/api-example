using Ardalis.Result;
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

public class CreateTaskTypeAsync_Test
{
    /// <summary>
    /// Check when in the proccess user not have access to tasktype 
    /// </summary>
    /// <returns>Error, Company Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUserNotAccessToTaskType_ReturnErrorCompanyNotFound()
    {
        //Declare variables
        Project project = new Project() { CompanyId = 1 };
        TaskType taskType = new() { Project = project };
        string userID = "UserId";

        //Create simulation test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _, out _);

        //Configuration test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)null);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetBySpecAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(Result.Success(new CompanyUser() { CompanyId = 0 }));

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Project()));

        //Vaidation Teste
        var result = await taskTypeService.CreateTaskTypeAsync(taskType, userID);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyNotFound,
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check when in the proccess taskType not fullfill all naming requerimengts
    /// </summary>
    /// <returns>Error Task Type Name Duplicated</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeNotNotFullFillAllNamingRequeriments_ReturnErrorTaskTypeNameDuplicated()
    {
        //Declare variables
        Project project = new Project() { CompanyId = 1 };
        TaskType taskType = new() { Name = "name", Project = project, Description = "Description" };
        string userID = "UserId";

        //Create simulation test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configuration test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Company()));

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(Result.Success(new CompanyUser() { CompanyId = 0 }));

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Project()));

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByDescriptionSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Vaidation Teste
        var result = await taskTypeService.CreateTaskTypeAsync(taskType, userID);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeNameDuplicated.Replace("[name]", taskType.Name),
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check when all the proccess is correct
    /// </summary>
    /// <returns>IsCorrect</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declare variables
        Project project = new Project() { CompanyId = 1 };
        TaskType taskType = new() { Name = "name", Project = project, Description = "Description" };
        string userID = "UserId";

        //Create simulation test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configuration test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Company()));

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(Result.Success(new CompanyUser() { CompanyId = 0 }));

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Project()));

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByDescriptionSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Vaidation Teste
        var result = await taskTypeService.CreateTaskTypeAsync(taskType, userID);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Check when in the proccess unexpected error
    /// </summary>
    /// <returns>TestError</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnTestError()
    {
        //Declare variables
        Project project = new Project() { CompanyId = 1 };
        TaskType taskType = new() { Name = "name", Project = project, Description = "Description" };
        string userID = "UserId";
        var errorMessage = "Teste Error";

        //Create simulation test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository, out _);

        //Configuration test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetBySpecAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByNameSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.AnyAsync(
            It.IsAny<TaskTypesByDescriptionSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Vaidation Teste
        var result = await taskTypeService.CreateTaskTypeAsync(taskType, userID);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}
