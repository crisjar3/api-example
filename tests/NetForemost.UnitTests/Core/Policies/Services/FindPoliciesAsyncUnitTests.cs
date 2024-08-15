using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Specifications.Policies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Policies.Services;

public class FindPoliciesAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Find all Policies
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindPoliciesAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var company = new Company() { Id = 1 };

        var policies = new List<Policy>()
        {
            new Policy(){Id = 1, CompanyId  = company.Id},
        };

        // Create the simulated service
        var policyService = ServiceUtilities.CreatePolicyService(
            out Mock<IAsyncRepository<Policy>> policyRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        // Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        policyRepository.Setup(policyRepository => policyRepository.ListAsync(
            It.IsAny<GetPolicyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policies);

        var result = await policyService.FindPoliciesAsync(company.Id);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Company does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Company Not Found </returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnCompanyNotFound()
    {
        // Declarations of variables
        var company = new Company() { Id = 1 };

        var policies = new List<Policy>()
        {
            new Policy(){Id = 1, CompanyId  = company.Id},
        };

        // Create the simulated service
        var policyService = ServiceUtilities.CreatePolicyService(
            out Mock<IAsyncRepository<Policy>> policyRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        // Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Company)null);

        policyRepository.Setup(policyRepository => policyRepository.ListAsync(
            It.IsAny<GetPolicyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policies);

        var result = await policyService.FindPoliciesAsync(company.Id);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindPolicies_ReturnError()
    {
        // Declarations of variables
        var testError = "Error to find policies";
        var company = new Company() { Id = 1 };

        var policies = new List<Policy>()
        {
            new Policy(){Id = 1, CompanyId  = company.Id},
        };

        // Create the simulated service
        var policyService = ServiceUtilities.CreatePolicyService(
            out Mock<IAsyncRepository<Policy>> policyRepository,
            out Mock<IAsyncRepository<Company>> companyRepository
            );

        // Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        policyRepository.Setup(policyRepository => policyRepository.ListAsync(
            It.IsAny<GetPolicyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await policyService.FindPoliciesAsync(company.Id);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
