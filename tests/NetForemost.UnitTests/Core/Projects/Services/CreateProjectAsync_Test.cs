using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;


namespace NetForemost.UnitTests.Core.Projects.Services;

public class CreateProjectAsync_Test
{
    /// <summary>
    /// Check if Company Not Found in the proccess
    /// </summary>
    /// <returns>error, Company Not Found </returns>

    [Fact]
    public async Task WhenCompanyNotFound_ReturnErrorCompanyNotFound()
    {
        //Declarate Variables
        Project project = new Project();
        User userId = new User();

        //Create Simulated Service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuration for test
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync
        (It.IsAny<int>(),
         It.IsAny<CancellationToken>())).
         ReturnsAsync((Company)null);

        var result = await projectService.CreateProjectAsync(project, userId.Id);


        //Validation for test
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }


    /// <summary>
    /// Check if the project start date is less than the end date
    /// </summary>
    /// <returns>error , Project Dates Incorrect</returns>
    [Fact]
    public async Task WhenStartDatesIsIncorrect_ReturnErrorProjectDatesIncorrect()
    {
        //Declaration Variables
        Company company = new Company();
        User user = new User();
        Project project = new Project() { StartedDate = DateTime.Now, EndEstimatedDate = DateTime.Parse("12-12-2000 00:00:00") };

        //Create simulated service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuracion for teste

        companyRepository.Setup(companyRepository => companyRepository.AnyAsync
                   (It.IsAny<int>(),
                    It.IsAny<CancellationToken>())).
                    ReturnsAsync(true);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //validation Test
        var result = await projectService.CreateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectDatesIncorrect, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }


    /// <summary>
    /// cheack when Project is duplicated in the company
    /// </summary>
    /// <returns>Error Project Name Duplicated</returns>
    [Fact]
    public async Task WhenProjectNameIsDuplicated_ReturnErrorProjectNameDuplicated()
    {
        //Declaration Variables
        Company company = new Company();
        User user = new User();
        Project project = new() { Id = 1, Name = "NetForemost", CompanyId = 1, StartedDate = DateTime.Parse("08-01-2023 00:00:00"), EndEstimatedDate = DateTime.Parse("08-06-2023 00:00:00") };
        //Create simulated service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuracion for test

        projectRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<GetProjectsByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        companyRepository.Setup(companyRepository => companyRepository.AnyAsync
                   (It.IsAny<int>(),
                    It.IsAny<CancellationToken>())).
                    ReturnsAsync(true);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //validation Test
        var result = await projectService.CreateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectNameDuplicated.Replace("[name]",
            project.Name), ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Verify if antire proccess is correct
    /// </summary>
    /// <returns>susccess if all is correct</returns>

    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration Variables
        Company company = new Company();
        User user = new User();
        Project project = new() { Id = 1, Name = "NetForemost", CompanyId = 1, StartedDate = DateTime.Parse("08-01-2023 00:00:00"), EndEstimatedDate = DateTime.Parse("08-06-2023 00:00:00") };
        //Create simulated service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuracion for test

        projectRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<GetProjectsByNameSpecification>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(false);

        companyRepository.Setup(companyRepository => companyRepository.AnyAsync
                       (It.IsAny<int>(),
                        It.IsAny<CancellationToken>())).
                        ReturnsAsync(true);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //validation Test
        var result = await projectService.CreateProjectAsync(project, user.Id);

        Assert.True(result.IsSuccess);
    }


    /// <summary>
    /// check if unexpected error
    /// </summary>
    /// <returns>Unexpected Error
    /// </returns>

    [Fact]
    public async Task WhenUnexpectedError_ReturnError()
    {
        //Declaration Variables
        var testError = "TEST ERROR";
        Company company = new Company();
        User user = new User();
        Project project = new() { Id = 1, Name = "NetForemost", CompanyId = 1, StartedDate = DateTime.Parse("08-01-2023 00:00:00"), EndEstimatedDate = DateTime.Parse("08-06-2023 00:00:00") };
        //Create simulated service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuracion for test
        projectRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<GetProjectsByNameSpecification>(),
                It.IsAny<CancellationToken>())).Throws(new Exception(testError));

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync
                       (It.IsAny<int>(),
                        It.IsAny<CancellationToken>())).
                        ReturnsAsync((Company)company);

        //validation Test
        var result = await projectService.CreateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }

}
