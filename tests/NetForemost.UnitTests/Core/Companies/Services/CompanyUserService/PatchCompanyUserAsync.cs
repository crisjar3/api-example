using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserService;

public class PatchCompanyUserAsync
{
    /// <summary>
    /// Verify if all process is correct
    /// </summary>
    /// <returns> is success</returns>
    [Fact]
    public async Task WhenPatchingCompanyUserComplete_ReturnSuccess()
    {
        //Variable Declaration
        string userId = "";
        int companyUserId = new();
        JsonPatchDocument<CompanyUser> patchCompanyUser = new();

        //Created Simulated service

        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out _,
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _);

        //Configuration of test 

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new CompanyUser());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await companyUserService.PatchCompanyUserAsync(companyUserId, patchCompanyUser, userId);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Verify reponse when companyUser not found
    /// </summary>
    /// <returns>invalidError company user not found</returns>
    [Fact]
    public async Task WhenCompanyUserNotFound_ReturnInvalid()
    {
        //Variable Declaration
        string userId = "";
        int companyUserId = new();
        JsonPatchDocument<CompanyUser> patchCompanyUser = new();

        //Created Simulated service

        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out _,
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _);

        //Configuration of test 

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)null);

        var result = await companyUserService.PatchCompanyUserAsync(companyUserId, patchCompanyUser, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyUserNotFound);
    }

    /// <summary>
    /// Verify when companyUse
    /// </summary>
    /// <returns>invalidError user not belong to company</returns>
    [Fact]
    public async Task WhenUserNotBelongToCompany_ReturnInvalid()
    {
        //Variable Declaration
        string userId = "";
        int companyUserId = new();
        JsonPatchDocument<CompanyUser> patchCompanyUser = new();

        //Created Simulated service

        var companyUserService = ServiceUtilities.CreateCompanyUserService(
            out _,
            out _,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out _,
            out _,
            out _,
            out _);

        //Configuration of test 

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((CompanyUser)new());

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.AnyAsync(
            It.IsAny<CheckUserIsInCompanySpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await companyUserService.PatchCompanyUserAsync(companyUserId, patchCompanyUser, userId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.UserDoesNotBelongToTheCompany);
    }
}
