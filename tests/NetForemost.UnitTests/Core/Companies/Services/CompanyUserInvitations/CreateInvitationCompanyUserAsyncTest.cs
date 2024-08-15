using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Interfaces.SendGrid;
using NetForemost.Core.Specifications.Projects;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserInvitations;

public class CreateInvitationCompanyUserAsyncTest
{
    /// <summary>
    /// Verify when project not belong to the company
    /// </summary>
    /// <returns>Validation Error</returns>
    [Fact]
    public async Task WhenAProjectDoesNotBelongToTheCompany_ReturnValidationError()
    {
        //Declarate Variables
        NetForemost.Core.Entities.Companies.CompanyUserInvitation companyUserInvitation = new()
        {
            CompanyId = 1,
        };
        IEnumerable<ProjectDtoSimple> projects = new[]
        {
            new ProjectDtoSimple()
            {
                Id = 1,
                Name = "TimeForemost"
            }
        };
        string userId = "njcnaundieadnuf9cn8793";

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
        out _,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out Mock<IAsyncRepository<Company>> companyRepository,
        out _,
        out Mock<RoleManager<Role>> roleManager,
        out Mock<IAsyncRepository<Project>> projectRepository,
        null);

        //Configuration Test
        roleManager.Setup(roleManager => roleManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(new Role());

        companyUserRepository.Setup(companyRepository => companyRepository.AnyAsync(
            It.IsAny<ValidateEmailBelongToCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)new());

        projectRepository.Setup(projectRepository => projectRepository.ListAsync(
            It.IsAny<GetCompanyProjectsSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProjectDtoSimple>());

        //Configuration Test
        var result = await companyUserInvitationServices.CreateInvitationCompanyUserAsync(companyUserInvitation, projects, userId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.ProjectNotBelongToCompany.Replace("[Name]", projects.FirstOrDefault().Name), ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList()));
    }

    /// <summary>
    /// Verify if companyNotFound
    /// </summary>
    /// <returns>Validation Error</returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnValidationError()
    {
        //Declarate Variables
        NetForemost.Core.Entities.Companies.CompanyUserInvitation companyUserInvitation = new()
        {
            CompanyId = 1,
        };
        IEnumerable<ProjectDtoSimple> projects = new[]
        {
            new ProjectDtoSimple()
            {
                Id = 1,
                Name = "TimeForemost"
            }
        };
        string userId = "njcnaundieadnuf9cn8793";

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
        out _,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out _,
        out _,
        out _,
        out _,
        null);

        //Configuration Test
        companyUserRepository.Setup(companyRepository => companyRepository.AnyAsync(
            It.IsAny<ValidateEmailBelongToCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        //Configuration Test
        var result = await companyUserInvitationServices.CreateInvitationCompanyUserAsync(companyUserInvitation, projects, userId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.CompanyNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList()));
    }

    /// <summary>
    /// verify if email belong to company
    /// </summary>
    /// <returns>Validation Error</returns>
    [Fact]
    public async Task WhenEmailBelongToCompany_ReturnValidationError()
    {
        //Declarate Variables
        NetForemost.Core.Entities.Companies.CompanyUserInvitation companyUserInvitation = new()
        {
            CompanyId = 1,
        };
        IEnumerable<ProjectDtoSimple> projects = new[]
        {
            new ProjectDtoSimple()
            {
                Id = 1,
                Name = "TimeForemost"
            }
        };
        string userId = "njcnaundieadnuf9cn8793";

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
        out _,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out _,
        out _,
        out _,
        out _,
        null);

        //Configuration Test
        companyUserRepository.Setup(companyRepository => companyRepository.AnyAsync(
            It.IsAny<ValidateEmailBelongToCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        //Configuration Test
        var result = await companyUserInvitationServices.CreateInvitationCompanyUserAsync(companyUserInvitation, projects, userId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.EmailExistsInTheCompany, ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList()));
    }

    /// <summary>
    /// Verify when role not found response
    /// </summary>
    /// <returns>validation error</returns>
    [Fact]
    public async Task WhenRoleNotFound_ReturnValidationError()
    {
        //Declarate Variables
        NetForemost.Core.Entities.Companies.CompanyUserInvitation companyUserInvitation = new()
        {
            CompanyId = 1,
        };
        IEnumerable<ProjectDtoSimple> projects = new[]
        {
            new ProjectDtoSimple()
            {
                Id = 1,
                Name = "TimeForemost"
            }
        };
        string userId = "njcnaundieadnuf9cn8793";

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
        out _,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out Mock<IAsyncRepository<Company>> companyRepository,
        out _,
        out Mock<RoleManager<Role>> roleManager,
        out _,
        null);

        //Configuration Test
        roleManager.Setup(roleManager => roleManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync((Role)null);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<ValidateEmailBelongToCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)new());

        //Configuration Test
        var result = await companyUserInvitationServices.CreateInvitationCompanyUserAsync(companyUserInvitation, projects, userId);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.RoleNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList()));
    }

    /// <summary>
    /// when sendgrind not send email
    /// </summary>
    /// <returns>UnexpectedError</returns>
    [Fact]
    public async Task WhenSendGrindNotSendEmail_ReturnError()
    {
        //Declarate Variables
        NetForemost.Core.Entities.Companies.CompanyUserInvitation companyUserInvitation = new()
        {
            CompanyId = 1,
        };
        IEnumerable<ProjectDtoSimple> projects = new[]
        {
            new ProjectDtoSimple()
            {
                Id = 1,
                Name = "TimeForemost"
            }
        };
        string userId = "njcnaundieadnuf9cn8793";
        string subject = " ";
        string htmlContent = " ";
        string textContent = " ";
        string emailTo = " ";
        string emailToName = " ";
        var sendGrindService = new Mock<ISendGridService>();

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
        out _,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out Mock<IAsyncRepository<Company>> companyRepository,
        out _,
        out Mock<RoleManager<Role>> roleManager,
        out Mock<IAsyncRepository<Project>> projectRepository,
        sendGrindService);

        //Configuration Test
        roleManager.Setup(roleManager => roleManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(new Role());

        companyUserRepository.Setup(companyRepository => companyRepository.AnyAsync(
            It.IsAny<ValidateEmailBelongToCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        companyRepository.Setup(companyRepository => companyRepository.AnyAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        projectRepository.Setup(projectRepository => projectRepository.ListAsync(
            It.IsAny<GetCompanyProjectsSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProjectDtoSimple>() {
                new ProjectDtoSimple()
                {
                    Id = 1,
                    Name = "TimeForemost"
                }
            });

        sendGrindService.Setup(sendGrindService => sendGrindService.TrySendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(false);

        //Configuration Test
        var result = await companyUserInvitationServices.CreateInvitationCompanyUserAsync(companyUserInvitation, projects, userId);

        Assert.False(result.IsSuccess);
        Assert.Contains("", ErrorHelper.GetErrors(result.Errors.ToList()));
    }

    /// <summary>
    /// Verify when proccess success
    /// </summary>
    /// <returns>Is Success</returns>
    [Fact]
    public async Task WhenProcessSuccesfull_ReturnSuccess()
    {
        //Declarate Variables
        NetForemost.Core.Entities.Companies.CompanyUserInvitation companyUserInvitation = new()
        {
            CompanyId = 1,
        };
        IEnumerable<ProjectDtoSimple> projects = new[]
        {
            new ProjectDtoSimple()
            {
                Id = 1,
                Name = "TimeForemost"
            }
        };
        string userId = "njcnaundieadnuf9cn8793";
        string subject = " ";
        string htmlContent = " ";
        string textContent = " ";
        string emailTo = " ";
        string emailToName = " ";
        var sendGrindService = new Mock<ISendGridService>();
        string testError = "Exception Error";
        NetForemost.Core.Entities.Companies.CompanyUserInvitation invitation = new();

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
        out Mock<IAsyncRepository<NetForemost.Core.Entities.Companies.CompanyUserInvitation>> companyUserInvitationRepository,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out Mock<IAsyncRepository<Company>> companyRepository,
        out _,
        out Mock<RoleManager<Role>> roleManager,
        out Mock<IAsyncRepository<Project>> projectRepository,
        sendGrindService);

        //Configuration Test
        roleManager.Setup(roleManager => roleManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(new Role());

        companyUserRepository.Setup(companyRepository => companyRepository.AnyAsync(
            It.IsAny<ValidateEmailBelongToCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((Company)new());

        projectRepository.Setup(projectRepository => projectRepository.ListAsync(
            It.IsAny<GetCompanyProjectsSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProjectDtoSimple>() {
                new ProjectDtoSimple()
                {
                    Id = 1,
                    Name = "TimeForemost"
                }
            });

        sendGrindService.Setup(sendGrindService => sendGrindService.TrySendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(true);

        companyUserInvitationRepository.Setup(companyUserInvitationRepository => companyUserInvitationRepository.AddAsync(
            It.IsAny<NetForemost.Core.Entities.Companies.CompanyUserInvitation>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(invitation);

        //Configuration Test
        var result = await companyUserInvitationServices.CreateInvitationCompanyUserAsync(companyUserInvitation, projects, userId);

        Assert.True(result.IsSuccess);
    }
}