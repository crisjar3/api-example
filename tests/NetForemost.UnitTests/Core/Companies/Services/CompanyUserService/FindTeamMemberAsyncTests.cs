using Moq;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserService;

public class FindMemberAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Find Member
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindMemberAsyncIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var user = new User() { Id = userId, };
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

        var timeZonesIds = new int[1];
        var CompanyUserIds = new int[1];
        var from = DateTime.MinValue;
        var to = DateTime.MaxValue;
        var perPage = 10;
        var pageNumber = 1;

        var companiesUser = new List<CompanyUser>() { companyUser };

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _, out _, out _, out _, out _);

        //Configurations for tests
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<GetUsersByCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<UserSettingCompanyUserDto>());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.CountAsync(
            It.IsAny<GetUsersByCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser.Count);

        var result = await companyUserService.FindTeamMembersAsync(companyId, timeZonesIds, CompanyUserIds, true, from, to, pageNumber, perPage, userId);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the User belong to the company and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return User not belong to the Company </returns>
    [Fact]
    public async Task WhenUserDoesNotBelongToTheCompany_ReturnUserNotBelongToTheCompany()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var user = new User() { Id = userId, };
        var company = new Company() { Id = companyId, Name = "Company Test" };
        var timeZonesIds = new int[1];
        var CompanyUserIds = new int[1];
        var from = DateTime.MinValue;
        var to = DateTime.MaxValue;
        var perPage = 10;
        var pageNumber = 1;

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
            out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _, out _, out _, out _, out _);

        //Configurations for tests
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<GetUsersByCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<UserSettingCompanyUserDto>());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.CountAsync(
            It.IsAny<GetUsersByCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser.Count);

        var result = await companyUserService.FindTeamMembersAsync(companyId, timeZonesIds, CompanyUserIds, true, from, to, pageNumber, perPage, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenUnexpectedErrosOccurToFinMemberAsync_ReturnError()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;
        var testError = "Error to FindMemberAsync";
        var user = new User() { Id = userId, };
        var company = new Company() { Id = companyId, Name = "Company Test" };
        var timeZonesIds = new int[1];
        var CompanyUserIds = new int[1];
        var from = DateTime.MinValue;
        var to = DateTime.MaxValue;
        var perPage = 10;
        var pageNumber = 1;

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
            out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _, out _, out _, out _, out _);

        //Configurations for tests
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<GetUsersByCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.CountAsync(
            It.IsAny<GetUsersByCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companiesUser.Count);

        var result = await companyUserService.FindTeamMembersAsync(companyId, timeZonesIds, CompanyUserIds, true, from, to, pageNumber, perPage, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}