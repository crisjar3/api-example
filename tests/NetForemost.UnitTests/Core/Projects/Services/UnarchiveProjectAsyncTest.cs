using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Companies;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Projects.Services;

public class UnUnarchiveProjectAsyncTest
{
    // Unarchive a project successfully when all conditions are met
    [Fact]
    public async Task WhenAllIsRight_ReturnSuccess()
    {
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> _companyUserRepository,
            out Mock<IAsyncRepository<Project>> _projectRepository,
            out _,
            out _,
            out _,
            out _, out _
        );

        var project = new Project
        {
            Id = 1,
            CompanyId = 1,
            Name = "Test Project",
            Description = "Test Description",
            Budget = 1000,
            StartedDate = DateTime.UtcNow,
            EndEstimatedDate = DateTime.UtcNow.AddDays(30),
            IsArchived = true
        };

        var userId = "testUser";

        _projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        _companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        _projectRepository.Setup(projectRepository => projectRepository.UpdateAsync(
            It.IsAny<Project>(),
            It.IsAny<CancellationToken>()
            )).Returns(Task.CompletedTask);

        // Act
        var result = await projectService.UnarchiveProjectAsync(userId, project.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.Value.IsArchived);
    }

    // Return an error when the project to unarchive is not found
    [Fact]
    public async Task WhenProjectToArchiveNotFound_ReturnAnError()
    {
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> _companyUserRepository,
            out Mock<IAsyncRepository<Project>> _projectRepository,
            out _,
            out _,
            out _,
            out _, out _
        );

        var userId = "testUser";
        var projectId = 1;

        _projectRepository.Setup(r => r.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Project)null);

        // Act
        var result = await projectService.UnarchiveProjectAsync(userId, projectId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.ProjectNotFound.Replace("[id]", projectId.ToString()), result.ValidationErrors.First().ErrorMessage);
    }

    // Return an error when the project to unarchive is already unarchived
    [Fact]
    public async Task WhenProjectToArchiveAlreadyUnarchived_ReturnAnAlreadyArchivedError()
    {
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> _companyUserRepository,
            out Mock<IAsyncRepository<Project>> _projectRepository,
            out _,
            out _,
            out _,
            out _, out _
        );

        var userId = "testUser";
        var projectId = 1;

        Project projectAlreadyArchived = new Project()
        {
            Id = 1,
            IsArchived = false
        };

        _projectRepository.Setup(r => r.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(projectAlreadyArchived);

        // Act
        var result = await projectService.UnarchiveProjectAsync(userId, projectId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.RegisterAlreadyUnarchived, result.Errors.First().ToString());
    }

    // Return an error when the project is from a company the user does not belong
    [Fact]
    public async Task WhenTheUserMakingTheRequestDoesNotBelongToCompany_ReturnAnUserDoesNotBelongToTheCompanyError()
    {
        var projectService = ServiceUtilities.CreateProjectServices(
            out _,
            out Mock<IAsyncRepository<CompanyUser>> _companyUserRepository,
            out Mock<IAsyncRepository<Project>> _projectRepository,
            out _,
            out _,
            out _,
            out _, out _
        );

        var userId = "testUser";

        Project project = new Project()
        {
            Id = 0,
            CompanyId = 18,
            IsArchived = true
        };

        _projectRepository.Setup(r => r.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(project);

        _companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(false);

        // Act
        var result = await projectService.UnarchiveProjectAsync(userId, project.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.UserDoesNotBelongToTheCompany, result.ValidationErrors.First().ErrorMessage);
    }
}