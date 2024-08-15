using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskTypeService;

public class ValidateAccessTaskTypeAsync_Tests
{
    /// <summary>
    /// Check when all proccess is correct
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declarate variables
        Project project = new() { CompanyId = 0 };
        TaskType taskType = new() { Project = project };
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _, out _);

        //Configure test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Company());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new CompanyUser() { CompanyId = 0 });

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Project());

        //Validations test
        var result = await tasktypeService.ValidateAccessTaskTypeAsync(taskType, userId);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    ///Check when in the proccess have unexpected error
    /// </summary>
    /// <returns>Test Error</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnErrorTestError()
    {
        //Declarate variables
        Project project = new() { CompanyId = 0 };
        TaskType taskType = new() { Project = project };
        string userId = "userId";
        var errorMessage = "Test Error";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _, out _);

        //Configure test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        //Validations test
        var result = await tasktypeService.ValidateAccessTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }

    /// <summary>
    ///Check when companyUser not found
    /// </summary>
    /// <returns>ErroStringsCompanyNotFound</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenCompanyNotFound_ReturnCompanyNotFound()
    {
        //Declarate variables
        Project project = new() { CompanyId = 0 };
        TaskType taskType = new() { Project = project };
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _, out _);

        //Configure test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)null);

        //Validations test
        var result = await tasktypeService.ValidateAccessTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    ///Check when User nor belong to the company
    /// </summary>
    /// <returns>Erro Company User Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUserNotBelongTocCompany_ReturnCompanyUserNotFound()
    {
        //Declarate variables
        Project project = new() { CompanyId = 0 };
        TaskType taskType = new() { Project = project };
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _, out _);

        //Configure test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Company() { Id = 1 });

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)null);

        //Validations test
        var result = await tasktypeService.ValidateAccessTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyUserNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    ///Check when project not belong to company
    /// </summary>
    /// <returns>Error Project Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenProjectNotBelongToCompany_ReturnProjectNotFound()
    {
        //Declarate variables
        Project project = new();
        TaskType taskType = new();
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _, out _);

        //Configure test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Company());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new CompanyUser());

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)null);

        //Validations test
        var result = await tasktypeService.ValidateAccessTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    ///Check when project not belong to company 
    /// </summary>
    /// <returns>Error Project Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenProjectNotBelongToCompanyOrCompanyUserNotHaveToSameCompanyId_ReturnProjectNotFound()
    {
        //Declarate variables
        Project project = new() { CompanyId = 1 };
        TaskType taskType = new() { Project = project };
        string userId = "userId";

        //Create simulated Tests
        var tasktypeService = ServiceUtilities.CreateTaskTypeService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _, out _);

        //Configure test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Company());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.FirstOrDefaultAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new CompanyUser() { CompanyId = 1 });

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new Project());

        //Validations test
        var result = await tasktypeService.ValidateAccessTaskTypeAsync(taskType, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectNotOwnedTheCompany, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }
}
