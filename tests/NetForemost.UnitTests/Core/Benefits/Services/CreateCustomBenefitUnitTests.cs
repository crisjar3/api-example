using Moq;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Benefits.Services;

public class CreateCustomBenefitUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Create Benefit Custom
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateCustomBenefitIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var benefit = new Benefit()
        {
            Id = 1,
            CompanyId = company.Id,
            Company = company
        };

        //Create the simulated service
        var benefitService = ServiceUtilities.CreateBenefitService(
            out Mock<IAsyncRepository<Benefit>> benefitsRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitsRepository.Setup(benefitsRepository => benefitsRepository.AddAsync(
            It.IsAny<Benefit>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        var result = await benefitService.CreateCustomBenefit(benefit, userId);

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
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var benefit = new Benefit()
        {
            Id = 1,
            CompanyId = company.Id,
            Company = company
        };

        //Create the simulated service
        var benefitService = ServiceUtilities.CreateBenefitService(
            out Mock<IAsyncRepository<Benefit>> benefitsRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Company)null);

        benefitsRepository.Setup(benefitsRepository => benefitsRepository.AddAsync(
            It.IsAny<Benefit>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        var result = await benefitService.CreateCustomBenefit(benefit, userId);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurWithCreateBenefit_ReturnError()
    {
        //Delcarations of variables
        var userId = "UserId";
        var testError = "An Error occurs while Create a Custom Benefit";
        var company = new Company() { Id = 1 };
        var benefit = new Benefit()
        {
            Id = 1,
            CompanyId = company.Id,
            Company = company
        };

        //Create the simulated service
        var benefitService = ServiceUtilities.CreateBenefitService(
            out Mock<IAsyncRepository<Benefit>> benefitsRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitsRepository.Setup(benefitsRepository => benefitsRepository.AddAsync(
            It.IsAny<Benefit>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await benefitService.CreateCustomBenefit(benefit, userId);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
