using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserService;

public class CreateTeamMemberAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create Team Member
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateTeamMemberAsyncIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        var company = new Company() { Id = companyId, Name = "Company Test" };
        var companyUser = new CompanyUser()
        {
            Id = 1,
            User = user,
            UserId = user.Id,
            Company = company,
            CompanyId = company.Id,
            IsActive = true,
            JobRoleId = 1
        };

        var companiesUser = new List<CompanyUser>() { companyUser };
        var projectList = new List<Project>()
        {
            new Project()
            {
                Id = 1,
                IsAccessibleForEveryone = true
            }
        };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository, out _,
            out Mock<IAsyncRepository<Project>> projectRepository);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(new List<CompanyUser>() { new CompanyUser() { UserId = userId, CompanyId = companyId, IsActive = true } });

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        roleManager.Setup(roleManager => roleManager.FindByIdAsync(
            It.IsAny<string?>()
            )).ReturnsAsync(role);

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new NetForemost.Core.Entities.TimeZones.TimeZone());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new JobRole());
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AddAsync(
            It.IsAny<CompanyUser>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companyUser);

        projectRepository.Setup(projectRepository => projectRepository.ListAsync(
            It.IsAny<GetProjectsAccesiblesForEveryone>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(projectList);

        var newCompanyUser = new CompanyUser()
        {
            UserId = userIdAdd,  // Asegúrate de que esto coincida con el usuario que deseas agregar
            CompanyId = companyId,
            RoleId = "18",
            TimeZoneId = 7,
            JobRoleId = 18
        };

        var result = await companyUserService.CreateTeamMemberAsync(newCompanyUser, userId);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Verify edge case when timezone not found
    /// </summary>
    /// <returns> Return Validation Error Timezone Not Found </returns>
    [Fact]
    public async Task WhenTimeZoneNotFound_ReturnValidationError()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        var company = new Company() { Id = companyId, Name = "Company Test" };
        var companyUser = new CompanyUser()
        {
            Id = 1,
            User = user,
            UserId = user.Id,
            Company = company,
            CompanyId = company.Id,
            IsActive = true
        };

        var companiesUser = new List<CompanyUser>() { companyUser };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        roleManager.Setup(roleManager => roleManager.FindByIdAsync(
            It.IsAny<string?>()
            )).ReturnsAsync(role);

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((int? id, CancellationToken cancellationToken) => null);

        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.TimeZoneNotExist);
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// Verify edge case when jobrole not found
    /// </summary>
    /// <returns> Return Validation Error jobrole Not Found </returns>
    [Fact]
    public async Task WhenJobRoleNotFound_ReturnValidationError()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        var company = new Company() { Id = companyId, Name = "Company Test" };
        var companyUser = new CompanyUser()
        {
            Id = 1,
            User = user,
            UserId = user.Id,
            Company = company,
            CompanyId = company.Id,
            IsActive = true
        };

        var companiesUser = new List<CompanyUser>() { companyUser };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        roleManager.Setup(roleManager => roleManager.FindByIdAsync(
            It.IsAny<string?>()
            )).ReturnsAsync(role);

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new NetForemost.Core.Entities.TimeZones.TimeZone());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((int? id, CancellationToken cancellationToken) => null);


        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobRoleIdNotFound);
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the User does'nt exist, So it doesn't finish the process correctly.
    /// </summary>
    /// <returns> Return User Not Found </returns>
    [Fact]
    public async Task WhenUserNotFound_ReturnUserNotFound()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var userIdNotExist = "Not exist";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out _, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            userIdNotExist
            )).ReturnsAsync(new User());

        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.User_NotFound);
    }

    /// <summary>
    /// It checks if the User does'nt available, So it doesn't finish the process correctly.
    /// </summary>
    /// <returns> Return User Not Available to add Team </returns>
    [Fact]
    public async Task WhenUserAddIsNotActive_ReturnUserNotAvailableToAddTeam()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };
        var userIsActive = false;

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = userIsActive,
        };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out _, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserNotAvailableToAddInTeam);
    }

    /// <summary>
    /// It checks if the Company does'nt exist, So it doesn't finish the process correctly.
    /// </summary>
    /// <returns> Return Company Not Found </returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnCompanyNotFound()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        var companyIdNotExist = 2;
        var company = new Company() { Id = companyId, Name = "Company Test" };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out _, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            companyIdNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Company());

        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if the User does'nt belong to the Company, So it doesn't finish the process correctly.
    /// </summary>
    /// <returns> Return User not Belong to the Company </returns>
    [Fact]
    public async Task WhenUserToCreateRecordsNotBelongToTheCompany_RetursUserNotBelongToTheCompany()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        var company = new Company() { Id = companyId, Name = "Company Test" };
        var companyUser = new CompanyUser()
        {
            Id = 1,
            User = user,
            UserId = user.Id,
            Company = company,
            CompanyId = company.Id,
            IsActive = true
        };

        var companiesUser = new List<CompanyUser>() { companyUser };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out _, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<CompanyUser>());

        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
    }

    /// <summary>
    /// It checks if the User exist in the Company, So it doesn't finish the process correctly.
    /// </summary>
    /// <returns> Return Talent User exist in the Company </returns>
    [Fact]
    public async Task WhenUserToAddExistInTheCompany_ReturnTalentExistInTheCompany()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        var company = new Company() { Id = companyId, Name = "Company Test" };
        var companyUser = new CompanyUser()
        {
            Id = 1,
            User = user,
            UserId = user.Id,
            Company = company,
            CompanyId = company.Id,
            IsActive = true
        };

        var companiesUser = new List<CompanyUser>() { companyUser };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out _, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        roleManager.Setup(roleManager => roleManager.FindByNameAsync(
            It.IsAny<string?>()
            )).ReturnsAsync(role);

        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.TalentExistsInTheCompany);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccur_ReturnError()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var userIdAdd = "2";
        var testError = "Error in CreateTeamService";
        var jobOffer = new JobOffer();
        var role = new Role();
        var user = new User() { Id = userId, };

        var userAdd = new User()
        {
            Id = userIdAdd,
            IsActive = true,
        };

        var company = new Company() { Id = companyId, Name = "Company Test" };
        var companyUser = new CompanyUser()
        {
            Id = 1,
            User = user,
            UserId = user.Id,
            Company = company,
            CompanyId = company.Id,
            IsActive = true
        };

        var companiesUser = new List<CompanyUser>() { companyUser };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out _, out _, out _);

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(userAdd);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        roleManager.Setup(roleManager => roleManager.FindByNameAsync(
            It.IsAny<string?>()
            )).ReturnsAsync(role);

        var result = await companyUserService.CreateTeamMemberAsync(new CompanyUser(), userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
