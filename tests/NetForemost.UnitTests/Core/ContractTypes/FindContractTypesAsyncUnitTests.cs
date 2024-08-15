using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Specifications.ContractTypes;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.ContractTypes;

public class FindContractTypesAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Find Contract Type
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindContractTypeAsyncIsCorrect_ReturnSuccess()
    {
        // Delcarations of variables
        var companyId = 1;
        var contractTypes = new List<ContractType>() { new ContractType() };
        var company = new Company() { Id = companyId };

        // Create the simulated service
        var contractTypeService = ServiceUtilities.CreateContractTypeService(
            out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        // Configurations for tests
        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.ListAsync(
            It.IsAny<GetContractTypeByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(contractTypes);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        var result = await contractTypeService.FindContractTypesAsync(companyId);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Company not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Company NotFound </returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnCompanyNotFound()
    {
        // Delcarations of variables
        var companyId = 1;

        // Create the simulated service
        var contractTypeService = ServiceUtilities.CreateContractTypeService(
            out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        // Configurations for tests
        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.ListAsync(
            It.IsAny<GetContractTypeByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<ContractType>());

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Company)null);

        var result = await contractTypeService.FindContractTypesAsync(companyId);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccur_ReturnError()
    {
        // Delcarations of variables
        var companyId = 1;
        var testError = "Error to find all ContractType";

        // Create the simulated service
        var contractTypeService = ServiceUtilities.CreateContractTypeService(
            out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        // Configurations for tests
        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.ListAsync(
            It.IsAny<GetContractTypeByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Company());

        var result = await contractTypeService.FindContractTypesAsync(companyId);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
