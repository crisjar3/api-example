using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserService;

public class VerifyUserCompanyAsyncTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Verify user company
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindMemberAsyncIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _, out _, out _, out _, out _);

        //Configurations for tests
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        var result = await companyUserService.VerifyUserCompanyAsync(userId, companyId);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the User not exist into the company and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return User not belong to the company </returns>
    [Fact]
    public async Task WhenUserNotExistInTheCompany_ReturnUserNotBelongToTheCompany()
    {
        //Delcarations of variables
        var userId = "1";
        var companyId = 1;

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _, out _, out _, out _, out _);

        //Configurations for tests
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        var result = await companyUserService.VerifyUserCompanyAsync(userId, companyId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenUnexpectedErrorsOccurToVerifiyUserToCompany_ReturnError()
    {
        //Delcarations of variables
        var userId = "1";
        var testError = "Error to verify if user belong to the company";
        var companyId = 1;

        //Create the simulated service
        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out _, out _, out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _, out _, out _, out _, out _);

        //Configurations for tests
        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await companyUserService.VerifyUserCompanyAsync(userId, companyId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}