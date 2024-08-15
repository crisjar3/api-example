using Moq;
using NetForemost.Core.Dtos.Tasks;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskTypeService;

public class GetTaskTypeRecentlyTest
{
    /// <summary>
    /// Check when all procceds is correct
    /// </summary>
    /// <returns>IsSuccess</returns>
    /// 
    [Fact]
    public async System.Threading.Tasks.Task WhenAllProcessIsCorrect_ReturnSuccess()
    {
        // Declaración de variables
        int ownerId = 12345;
        string search = "Testing";
        int pageNumber = 10;
        int perPage = 1;
        int projectId = 1;

        // Crear servicio simulado
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository
        );

        // Configurar el repositorio para devolver una lista vacía
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new CompanyUser());

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Project());

        taskRepository.Setup(repo => repo.ListAsync(
                It.IsAny<GetTaskTypesRecentlySpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(new List<GetTaskTypesDto>());

        // Validar la llamada al servicio
        var result = await taskTypeService.GetTaskTypeRecentAsync(ownerId, projectId, search, pageNumber, perPage);

        // Assert
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// When the company user does not exist, the process does not continue
    /// </summary>
    /// <returns>Company User Not Found</returns>
    /// 
    [Fact]
    public async System.Threading.Tasks.Task WhenCompanyUserNotFoud_ReturnCompanyUserNotFoud()
    {
        // Declaración de variables
        int ownerId = 12345;
        string search = "Testing";
        int pageNumber = 10;
        int perPage = 1;
        int projectId = 1;

        // Crear servicio simulado
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository
        );

        // Configurar el repositorio para devolver una lista vacía
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((CompanyUser)null);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Project());

        taskRepository.Setup(repo => repo.ListAsync(
                It.IsAny<GetTaskTypesRecentlySpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(new List<GetTaskTypesDto>());

        // Validar la llamada al servicio
        var result = await taskTypeService.GetTaskTypeRecentAsync(ownerId, projectId, search, pageNumber, perPage);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyUserNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// When the project does not exist, the process does not continue
    /// </summary>
    /// <returns>Project Not Found</returns>
    /// 
    [Fact]
    public async System.Threading.Tasks.Task WhenProjectNotFoud_ReturnProjectNotFoud()
    {
        // Declaración de variables
        int ownerId = 12345;
        string search = "Testing";
        int pageNumber = 10;
        int perPage = 1;
        int projectId = 1;

        // Crear servicio simulado
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository
        );

        // Configurar el repositorio para devolver una lista vacía
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new CompanyUser());

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Project)null);

        taskRepository.Setup(repo => repo.ListAsync(
                It.IsAny<GetTaskTypesRecentlySpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(new List<GetTaskTypesDto>());

        // Validar la llamada al servicio
        var result = await taskTypeService.GetTaskTypeRecentAsync(ownerId, projectId, search, pageNumber, perPage);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }


    /// <summary>
    /// Check when in proccess is unexpected error
    /// </summary>
    /// <returns>UnexpectedError</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnError()
    {
        //DeclarationVariables
        int ownerId = 12345;
        string search = "Testing";
        int pageNumber = 10;
        int perPage = 1;
        int projectId = 1;
        var messageError = "Test Error";

        //Create simulated test
        var taskTypeService = ServiceUtilities.CreateTaskTypeService(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository
        );

        //Configuration Test
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new CompanyUser());

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Project());

        taskRepository.Setup(taskTypeRepository => taskTypeRepository.ListAsync(
            It.IsAny<GetTaskTypesRecentlySpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(messageError));

        //Validations Test
        var result = await taskTypeService.GetTaskTypeRecentAsync(ownerId, projectId, search, pageNumber, perPage);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), messageError);
    }
}