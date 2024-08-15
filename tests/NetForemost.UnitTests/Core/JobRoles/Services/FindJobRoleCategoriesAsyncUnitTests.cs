using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Specifications.JobRoles;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.JobRoles.Services;

public class FindJobRoleCategoriesAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create a JobRole
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task FindJobRoleCategoriesAsyncIsCorrect_ReturnSuccess()
    {
        //Declarations of variables
        var company = new Company() { Id = 1 };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository, out _, out _
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.ListAsync(
            It.IsAny<GetJobRoleCatgoriesWithJobRolesSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<JobRoleCategory>());

        var result = await jobRoleService.FindJobRoleCategoriesAsync(company.Id);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Company does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Company NotFound </returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnCompanyNotFound()
    {
        //Declarations of variables
        var company = new Company() { Id = 1 };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository, out _, out _
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Company)null);

        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.ListAsync(
            It.IsAny<GetJobRoleCatgoriesWithJobRolesSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new List<JobRoleCategory>());

        var result = await jobRoleService.FindJobRoleCategoriesAsync(company.Id);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if the Company does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Company NotFound </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindJobRoleCategory_ReturnError()
    {
        //Declarations of variables
        var company = new Company() { Id = 1 };
        var testError = "Error to find JobRoleCategory";

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository, out _, out _
            );

        //Configurations for tests
        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.ListAsync(
            It.IsAny<GetJobRoleCatgoriesWithJobRolesSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await jobRoleService.FindJobRoleCategoriesAsync(company.Id);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
