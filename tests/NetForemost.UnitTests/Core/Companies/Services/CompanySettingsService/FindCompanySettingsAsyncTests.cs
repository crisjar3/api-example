using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanySettingsService;

public class FindCompanySettingsAsyncTests
{
    /// <summary>
    /// check if not exist configuration fot the company
    /// </summary>
    /// <returns>error Company Settings Not Found</returns>
    [Fact]
    public async Task WhenNotExistConfigurationForCompany_ReturnErrorCompanySettingsNotFound()
    {
        //Declaration of Varibles
        CompanySettings companySettings = new CompanySettings();
        User user = new User();
        Company company = new Company();

        //Create Simulated service
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
             out _,
             out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
            );

        //Configuration For test
        companySettingsRepository.Setup(companySettingsRepository => companySettingsRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanySettingsByUserIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync((CompanySettings)null);

        var result = await companySettingsService.FindCompanySettingsAsync(user.Id);

        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanySettingsNotFound, ErrorHelper.GetErrors(result.Errors.ToList()));
    }

    /// <summary>
    /// check return UnExpected error
    /// </summary>
    /// <returns>Test error</returns>
    [Fact]
    public async Task WhenUnExpectedError_ReturnError()
    {
        //Declaration of Varibles
        CompanySettings companySettings = new CompanySettings();
        User user = new User();
        Company company = new Company();

        //Create Simulated service
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
             out _,
             out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
            );

        //Configuration For test
        companySettingsRepository.Setup(companySettingsRepository => companySettingsRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanySettingsByUserIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync((CompanySettings)null);

        var result = await companySettingsService.FindCompanySettingsAsync(user.Id);

        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), ErrorStrings.CompanySettingsNotFound);
    }

    /// <summary>
    /// check when all proccess of function is correct
    /// </summary>
    /// <returns>isSuccess</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSucess()
    {
        //Declaration of Varibles
        CompanySettings companySettings = new CompanySettings();
        User user = new User();
        Company company = new Company();

        //Create Simulated service
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
             out _,
             out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
            );

        //Configuration For test
        companySettingsRepository.Setup(companySettingsRepository => companySettingsRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanySettingsByUserIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(companySettings);

        var result = await companySettingsService.FindCompanySettingsAsync(user.Id);

        //Validation for teste
        Assert.True(result.IsSuccess);
    }
}