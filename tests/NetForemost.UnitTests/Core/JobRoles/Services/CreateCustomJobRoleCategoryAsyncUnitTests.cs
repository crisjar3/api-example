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

public class CreateCustomJobRoleCategoryAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create a JobRole
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateCustomJobRoleCategoryAsyncIsCorrect_ReturnSuccess()
    {
        //Declarations of variables
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1 };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository, out _, out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AnyAsync(
            It.IsAny<GetJobRoleCategoriesByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AddAsync(
            It.IsAny<JobRoleCategory>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRoleCategory);

        var result = await jobRoleService.CreateCustomJobRoleCategoryAsync(jobRoleCategory, userId);

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
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1 };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository, out _, out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AnyAsync(
            It.IsAny<GetJobRoleCategoriesByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Company)null);

        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AddAsync(
            It.IsAny<JobRoleCategory>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRoleCategory);

        var result = await jobRoleService.CreateCustomJobRoleCategoryAsync(jobRoleCategory, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if the JobRoleCategory does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return JobRoleCategory duplicated </returns>
    [Fact]
    public async Task WhenExistOtherJobRoleCategoryWithTheSameName_ReturnJobRoleCategoryDuplicated()
    {
        //Declarations of variables
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1, Name = "JobRoleCategoryName Test" };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository, out _, out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AnyAsync(
            It.IsAny<GetJobRoleCategoriesByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AddAsync(
            It.IsAny<JobRoleCategory>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRoleCategory);

        var result = await jobRoleService.CreateCustomJobRoleCategoryAsync(jobRoleCategory, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobRoleCategoryDuplicated.Replace("[name]", jobRoleCategory.Name));
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurWithCreateJobRoleCategory_ReturnError()
    {
        //Declarations of variables
        var testError = "Error to create JobRoleCategory";
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1, Name = "JobRoleCategoryName Test" };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository, out _, out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AnyAsync(
            It.IsAny<GetJobRoleCategoriesByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.AddAsync(
            It.IsAny<JobRoleCategory>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await jobRoleService.CreateCustomJobRoleCategoryAsync(jobRoleCategory, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
