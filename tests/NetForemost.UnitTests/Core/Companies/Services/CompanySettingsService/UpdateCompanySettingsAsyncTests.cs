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

public class UpdateCompanySettingsAsyncTests
{
    /// <summary>
    /// Verifi if company not exist
    /// </summary>
    /// <returns>Error Company Not Registed</returns>
    [Fact]
    public async Task WhenCompanyNotCreated_ReturnErrorCompanySettingsNotFound()
    {
        //Declaration of Varibles
        CompanySettings newcompanySettings = new CompanySettings();
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

        var result = await companySettingsService.UpdateCompanySettingsAsync(newcompanySettings, user.Id);

        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanySettingsNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Verifi if company not exist
    /// </summary>
    /// <returns>Error Company Not Registed</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration of Varibles
        CompanySettings newcompanySettings = new CompanySettings();
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
            ReturnsAsync(newcompanySettings);

        var result = await companySettingsService.UpdateCompanySettingsAsync(newcompanySettings, user.Id);

        //Validation for teste
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// When proccess request unexpected error
    /// </summary>
    /// <returns>Error Test</returns>
    [Fact]
    public async Task WhenUnExpectedError_ReturnError()
    {
        //Declaration of Varibles
        CompanySettings newcompanySettings = new CompanySettings();
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

        var result = await companySettingsService.UpdateCompanySettingsAsync(newcompanySettings, user.Id);
        var testError = "TEST ERROR";

        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}