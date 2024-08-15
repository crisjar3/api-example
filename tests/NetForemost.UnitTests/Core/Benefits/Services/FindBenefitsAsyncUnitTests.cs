using Moq;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Specifications.Benefits;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Benefits.Services;

public class FindBenefitsAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Find Benefits
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindBenefitsAsyncIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var company = new Company() { Id = 1 };

        //Create the simulated service
        var benefitService = ServiceUtilities.CreateBenefitService(
            out Mock<IAsyncRepository<Benefit>> benefitsRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitsRepository.Setup(benefitsRepository => benefitsRepository.ListAsync(
            It.IsAny<GetBenefitsByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<Benefit>());

        var result = await benefitService.FindBenefitsAsync(company.Id);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Company does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Company NotFound </returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnCompanyNotFound()
    {
        //Delcarations of variables
        var company = new Company() { Id = 1 };

        //Create the simulated service
        var benefitService = ServiceUtilities.CreateBenefitService(
            out Mock<IAsyncRepository<Benefit>> benefitsRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Company)null);

        benefitsRepository.Setup(benefitsRepository => benefitsRepository.ListAsync(
            It.IsAny<GetBenefitsByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<Benefit>());

        var result = await benefitService.FindBenefitsAsync(company.Id);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurWithFindBenefitsAsync_ReturnError()
    {
        //Delcarations of variables
        var company = new Company() { Id = 1 };
        var testError = "An Error occurs while found Benefits";

        //Create the simulated service
        var benefitService = ServiceUtilities.CreateBenefitService(
            out Mock<IAsyncRepository<Benefit>> benefitsRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitsRepository.Setup(benefitsRepository => benefitsRepository.ListAsync(
            It.IsAny<GetBenefitsByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await benefitService.FindBenefitsAsync(company.Id);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
