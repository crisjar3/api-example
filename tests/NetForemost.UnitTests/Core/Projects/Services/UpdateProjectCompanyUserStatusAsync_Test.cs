using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Projects.Services;

public class UpdateProjectCompanyUserStatusAsync_Test
{

    /// <summary>
    /// Check if all proccess is correct
    /// </summary>
    /// <returns>Success, when all proccess is correct</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnSuccess()
    {
        //Declarate Variables
        Project project = new();
        Company company = new();
        User user = new();
        ProjectCompanyUser projectCompanyUser = new() { Project = new Project() { CompanyId = 18 } };
        //Create service
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out _,
            out _,
            out _, out _);
        //Configuration test
        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.FirstOrDefaultAsync(
            It.IsAny<GetProjectCompanyUserByCompanyUserIdSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(projectCompanyUser);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);



        var result = await projectService.UpdateProjectCompanyUserStatusAsync(project.Id, company.Id, user.Id);

        //Validation Test
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Chenck when in proccess have Unexpected Error
    /// </summary>
    /// <returns>unexpected Error</returns>
    [Fact]
    public async Task WhenUnexpectedError_ReturnError()
    {
        //Declarate Variables
        Project project = new();
        Company company = new();
        User user = new();
        ProjectCompanyUser projectCompanyUser = new();
        var errorMessage = "Test Error";

        //Create service
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out _,
            out _,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out _,
            out _,
            out _, out _);
        //Configuration test
        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.FirstOrDefaultAsync(
            It.IsAny<GetProjectCompanyUserByCompanyUserIdSpecification>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        var result = await projectService.UpdateProjectCompanyUserStatusAsync(project.Id, company.Id, user.Id);

        //Validation Test
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}

