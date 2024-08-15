using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Specifications.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanySettingsService;
public class CreateCompanySettingsAsyncTests
{
    /// <summary>
    /// Verifi if company not exist
    /// </summary>
    /// <returns>error Company Not Registed</returns>
    [Fact]
    public async Task WhenCompanyNotRegistered_ReturnErrorCompanyNotRegistered()
    {
        //Declaration of Varibles
        var companyId = 1;
        CompanySettings companySettings = new CompanySettings();
        User user = new User();
        Company company = new Company() { Id = companyId };

        //Create Simulated service
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
             out Mock<ICompanyService> companyService,
             out _
            );

        //Configuration For test
        companyService.Setup(companyService => companyService.FindCompanyAsync(
            It.IsAny<int>())).
            ReturnsAsync((Company)null);

        var result = await companySettingsService.CreateCompanySettingsAsync(companySettings, user.Id, companyId);
        Console.WriteLine("Final del test");
        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyNotRegistered, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }
    
    /// <summary>
    /// Verifi if setting created Previously 
    /// </summary>
    /// <returns>error One Settings For Company </returns>
    [Fact]
    public async Task WhenCompanySettingsCreated_ReturnErrorOneSettingsForCompany()
    {
        //Declaration of Varibles
        var companyId = 1;
        CompanySettings companySettings = new CompanySettings();
        User user = new User();
        Company company = new Company() { Id = companyId };

        //Create Simulated service
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
             out Mock<ICompanyService> companyService,
             out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
            );

        //Configuration For test
        companyService.Setup(companyService => companyService.FindCompanyAsync(
            It.IsAny<int>())).
            ReturnsAsync(company);

        companySettingsRepository.Setup(companySettingsRepository => companySettingsRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanySettingsByUserIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(companySettings);

        var result = await companySettingsService.CreateCompanySettingsAsync(companySettings, user.Id, companyId);

        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.OneSettingsForCompany, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// check if all is correct in the method
    /// </summary>
    /// <returns> isSucess</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration of Varibles
        var companyId = 1;
        CompanySettings companySettings = new CompanySettings();
        User user = new User();
        Company company = new Company() { Id = companyId };

        //Create Simulated service
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
             out Mock<ICompanyService> companyService,
             out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
            );

        //Configuration For test
        companyService.Setup(companyService => companyService.FindCompanyAsync(
            It.IsAny<int>())).
            ReturnsAsync(company);

        companySettingsRepository.Setup(companySettingsRepository => companySettingsRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanySettingsByUserIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync((CompanySettings)null);

        companySettingsRepository.Setup(companySettingsRepository => companySettingsRepository.AddAsync(
            It.IsAny<CompanySettings>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companySettings);

        var result = await companySettingsService.CreateCompanySettingsAsync(companySettings, user.Id, companyId);

        //Validation for teste
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Check if UnexpectedErrorOccurs
    /// </summary>
    /// <returns>Test Errror</returns>
    [Fact]
    public async Task WhenExceptionError_ReturnError()
    {
        //Declaration of Varibles
        var companyId = 1;
        CompanySettings companySettings = new CompanySettings();
        User user = new User();
        Company company = new Company() { Id = companyId };
        var testError = "TEST ERROR";

        //Create Simulated service
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
         out Mock<ICompanyService> companyService,
         out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository);

        //Configuration For test
        companyService.Setup(companyService => companyService.FindCompanyAsync(
            It.IsAny<int>())).
            Throws(new Exception(testError));

        companySettingsRepository.Setup(companySettingsRepository => companySettingsRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanySettingsByUserIdSpecification>(),
            It.IsAny<CancellationToken>())).
            Throws(new Exception(testError));

        var result = await companySettingsService.CreateCompanySettingsAsync(companySettings, user.Id, companyId);

        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}