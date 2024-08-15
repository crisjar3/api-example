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

public class UpdateProjectAsyc_Test
{
    /// <summary>
    /// check error when Project Not Found
    /// </summary>
    /// <returns>ErrorString.ProjectNotFound</returns>
    [Fact]
    public async Task WhenProjectNotFound_ReturnErrorProjectNotFound()
    {
        //Declaration Variable
        Project project = new();
        User user = new();

        //Create Simulated Service
        var projectService = ServiceUtilities.CreateProjectServices(
             out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuration for test
        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)null);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validation Test
        var result = await projectService.UpdateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectNotFound.Replace("[id]", project.Id.ToString()), ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check error when CompanyNotFound
    /// </summary>
    /// <returns>ErrorString.CompanyNotFound</returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnErrorCompanyNotFound()
    {
        //Declaration Variable
        Project project = new();
        User user = new();

        //Create Simulated Service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuration for test
        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)null);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);


        //Validation Test
        var result = await projectService.UpdateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check result whhen starte date is less or equal to endestimateDate
    /// </summary>
    /// <returns> ErrorStrings.ProjectDatesIncorrect</returns>
    [Fact]
    public async Task WhenProjectDatesIncorrect_ReturnErrorProjectDatesIncorrect()
    {
        //Declaration Variable
        Project project = new() { StartedDate = DateTime.Parse("9/12/2023"), EndEstimatedDate = DateTime.Parse("9/10/2023") };
        User user = new();
        Company company = new();

        //Create Simulated Service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuration for test
        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)company);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validation Test
        var result = await projectService.UpdateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectDatesIncorrect, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }



    /// <summary>
    /// Check result when project name is dplicate 
    /// </summary>
    /// <returns> ErrorString.ProjectNameDuplicated</returns>
    [Fact]
    public async Task WhenProjectDuplicate_ReturnErrorProjectNameDuplicate()
    {
        //Declaration Variable
        Project project = new() { StartedDate = DateTime.Parse("9/01/2023"), EndEstimatedDate = DateTime.Parse("9/10/2023") };
        User user = new();
        Company company = new();

        //Create Simulated Service
        var projectService = ServiceUtilities.CreateProjectServices(
           out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuration for test
        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)company);

        projectRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<GetProjectsByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validation Test
        var result = await projectService.UpdateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectNameDuplicated.Replace("[name]", project.Name), ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// When procces catch UnExpected error
    /// </summary>
    /// <returns>Unexpected error</returns>
    [Fact]
    public async Task WhenUnexpectedError_ReturnError()
    {
        //Declaration Variable
        Project project = new() { StartedDate = DateTime.Parse("9/01/2023"), EndEstimatedDate = DateTime.Parse("9/10/2023") };
        User user = new();
        Company company = new();
        var testError = "TEST ERROR";

        //Create Simulated Service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuration for test
        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(testError));

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)company);

        projectRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<GetProjectsByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Validation Test
        var result = await projectService.UpdateProjectAsync(project, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }


    /// <summary>
    /// Verify if all proccess are corrects 
    /// </summary>
    /// <returns>Success when all proccess is correct</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration Variable
        Project project = new() { StartedDate = DateTime.Parse("9/01/2023"), EndEstimatedDate = DateTime.Parse("9/10/2023") };
        User user = new();
        Company company = new();

        //Create Simulated Service
        var projectService = ServiceUtilities.CreateProjectServices(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out _,
            out _,
            out _, out _);

        //Configuration for test
        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)company);

        projectRepository.Setup(projectRepository => projectRepository.AnyAsync(
            It.IsAny<GetProjectsByNameSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);



        //Validation Test
        var result = await projectService.UpdateProjectAsync(project, user.Id);

        Assert.True(result.IsSuccess);
    }

}
