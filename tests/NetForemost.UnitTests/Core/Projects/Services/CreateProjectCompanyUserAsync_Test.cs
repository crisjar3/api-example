using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.JobRoles;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Projects.Services;

public class CreateProjectCompanyUserAsync_Test
{

    /// <summary>
    /// check result when CompanyUser Not Found
    /// </summary>
    /// <returns>Error, Company User Not Found</returns>
    [Fact]
    public async Task WhenCompanyUserNotFound_ReturnErrorCompanyUserNotFound()
    {
        //Declaration Variables
        ProjectCompanyUser projectCompanyUser = new();
        User user = new();

        //Create simulated service
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _, out _);

        //configuration Test
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)null);

        //validated test

        var result = await projectService.CreateProjectCompanyUserAsync(projectCompanyUser, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyUserNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check result when Job Role Id Not Found
    /// </summary>
    /// <returns>Error, Job Role Id Not Found</returns>
    [Fact]
    public async Task WhenJobRoleIdNotFound_ReturnErrorJobRoleIdNotFound()
    {
        //Declaration Variables

        ProjectCompanyUser projectCompanyUser = new();
        User user = new();
        CompanyUser companyUser = new();
        //Create simulated service

        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepositor,
            out _,
            out _, out _);
        //configuration Test

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)companyUser);
        jobRoleRepositor.Setup(jobRoleRepositor => jobRoleRepositor.AnyAsync(
            It.IsAny<GetJobRoleByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //validated test

        var result = await projectService.CreateProjectCompanyUserAsync(projectCompanyUser, user.Id);


        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.JobRoleIdNotFound.Replace("[id]", projectCompanyUser.JobRoleId.ToString()), ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check result when ProjectNotFound
    /// </summary>
    /// <returns>Error, Project Not Found</returns>
    [Fact]
    public async Task WhenProjectNotFound_ReturnErrorProjectNotFound()
    {
        //Declaration Variables

        ProjectCompanyUser projectCompanyUser = new();
        User user = new();
        CompanyUser companyUser = new();
        //Create simulated service

        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out _,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepositor,
            out _,
            out _, out _);
        //configuration Test

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)companyUser);

        jobRoleRepositor.Setup(jobRoleRepositor => jobRoleRepositor.AnyAsync(
            It.IsAny<GetJobRoleByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)null);
        //validated test

        var result = await projectService.CreateProjectCompanyUserAsync(projectCompanyUser, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check if Project Company User Duplicated
    /// </summary>
    /// <returns>Error Project Not Found</returns>
    [Fact]
    public async Task WhenProjectCompanyUserDuplicated_ReturnErrorProjectCompanyUserDuplicated()
    {
        //Declaration Variables

        ProjectCompanyUser projectCompanyUser = new();
        User user = new();
        CompanyUser companyUser = new();
        Project project = new();
        //Create simulated service

        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepositor,
            out _,
            out _, out _);
        //configuration Test

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)companyUser);

        jobRoleRepositor.Setup(jobRoleRepositor => jobRoleRepositor.AnyAsync(
            It.IsAny<GetJobRoleByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.AnyAsync(
            It.IsAny<GetProjectCompanyUserByCompanyUserIdSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);
        //validated test

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await projectService.CreateProjectCompanyUserAsync(projectCompanyUser, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.ProjectCompanyUserDuplicated, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check if unexpected error
    /// </summary>
    /// <returns>UnexpectedError</returns>
    [Fact]
    public async Task WhenUnExpectedError_ReturnError()
    {
        //Declaration Variables

        ProjectCompanyUser projectCompanyUser = new();
        User user = new();
        CompanyUser companyUser = new();
        Project project = new();
        var errorMessage = "Test Error";
        //Create simulated service

        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepositor,
            out _,
            out _, out _);
        //configuration Test

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        jobRoleRepositor.Setup(jobRoleRepositor => jobRoleRepositor.AnyAsync(
            It.IsAny<GetJobRoleByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.AnyAsync(
            It.IsAny<GetProjectCompanyUserByCompanyUserIdSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));
        //validated test

        var result = await projectService.CreateProjectCompanyUserAsync(projectCompanyUser, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }

    /// <summary>
    /// check all is correct
    /// </summary>
    /// <returns>Success , when all proccess is correct</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration Variables
        ProjectCompanyUser projectCompanyUser = new();
        User user = new();
        CompanyUser companyUser = new();
        Project project = new();
        //Create simulated service

        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepositor,
            out _,
            out _, out _);
        //configuration Test

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)companyUser);

        jobRoleRepositor.Setup(jobRoleRepositor => jobRoleRepositor.AnyAsync(
            It.IsAny<GetJobRoleByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Project)project);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.AnyAsync(
            It.IsAny<GetProjectCompanyUserByCompanyUserIdSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //validated test

        var result = await projectService.CreateProjectCompanyUserAsync(projectCompanyUser, user.Id);

        Assert.True(result.IsSuccess);
    }

}
