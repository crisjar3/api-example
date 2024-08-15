using Moq;
using NetForemost.Core.Dtos.Companies.CompanyUserInvitations;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyUserInvitation;

public class FindInvitationByCompanyAsyncTest
{
    /// <summary>
    /// Test when companyNot Found in database
    /// </summary>
    /// <returns>Invalid error</returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnINvalidError()
    {
        //Declarate Variables
        int companyId = 18;
        int perPage = 100;
        int pageNumber = 1;

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
       out _,
       out _,
       out Mock<IAsyncRepository<Company>> companyRepository,
       out _,
       out _,
       out _,
       null);

        //Configuration Test

        companyRepository.Setup(company => company.AnyAsync(
        It.IsAny<int>(),
        It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await companyUserInvitationServices.FindInvitationByCompanyAsync(companyId, perPage, pageNumber);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.CompanyNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors.ToList()));
    }

    /// <summary>
    /// Test when all process is compalete
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async Task WhenAllProcessComplete_ReturnSuccess()
    {
        //Declarate Variables
        int companyId = 18;
        int perPage = 100;
        int pageNumber = 1;

        //Created Simulated Service
        var companyUserInvitationServices = ServiceUtilities.CreateCompanyUserInvitationService(
       out Mock<IAsyncRepository<NetForemost.Core.Entities.Companies.CompanyUserInvitation>> companyUserInvitationRepository,
       out _,
       out Mock<IAsyncRepository<Company>> companyRepository,
       out _,
       out _,
       out _,
       null);

        //Configuration Test

        companyRepository.Setup(company => company.AnyAsync(
        It.IsAny<int>(),
        It.IsAny<CancellationToken>())).ReturnsAsync(true);

        companyUserInvitationRepository.Setup(companyUserInvitationRepository => companyUserInvitationRepository.CountAsync(
            It.IsAny<GetCompanyUserInvitationSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(20);

        companyUserInvitationRepository.Setup(companyUserInvitationRepository => companyUserInvitationRepository.ListAsync(
            It.IsAny<GetCompanyUserInvitationSpecification>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((List<CompanyUserInvitationCompleteDto>)new());

        var result = await companyUserInvitationServices.FindInvitationByCompanyAsync(companyId, perPage, pageNumber);

        Assert.True(result.IsSuccess);
    }
}
