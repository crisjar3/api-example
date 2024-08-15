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

public class CreateCustomJobRoleAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create a JobRole
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateCustomJobRoleAsyncIsCorrect_ReturnSuccess()
    {
        //Declarations of variables
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1 };
        var jobRole = new JobRole() { Id = 1, JobRoleCategoryId = jobRoleCategory.Id, CompanyId = company.Id };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRoleCategory);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AnyAsync(
            It.IsAny<GetJobroleByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AddAsync(
            It.IsAny<JobRole>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        var result = await jobRoleService.CreateCustomJobRoleAsync(jobRole, userId);

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
        var jobRole = new JobRole() { Id = 1, JobRoleCategoryId = jobRoleCategory.Id, CompanyId = company.Id };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRoleCategory);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((Company)null);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AnyAsync(
            It.IsAny<GetJobroleByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AddAsync(
            It.IsAny<JobRole>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        var result = await jobRoleService.CreateCustomJobRoleAsync(jobRole, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if the JobRole does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return JobRole duplicated </returns>
    [Fact]
    public async Task WhenExistOtherJobRoleWithTheSameName_ReturnJobRoleDuplicated()
    {
        //Declarations of variables
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1 };
        var jobRole = new JobRole() { Id = 1, JobRoleCategoryId = jobRoleCategory.Id, CompanyId = company.Id };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((JobRoleCategory)null);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AnyAsync(
            It.IsAny<GetJobroleByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AddAsync(
            It.IsAny<JobRole>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        var result = await jobRoleService.CreateCustomJobRoleAsync(jobRole, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobRoleCategoryNotFound.Replace("[id]", jobRole.JobRoleCategoryId.ToString()));
    }

    /// <summary>
    /// It checks if the JobRoleCategory does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return JobRole Category NotFound </returns>
    [Fact]
    public async Task WhenJobRoleCategoryNotFound_ReturnJobRoleCategoryNotFound()
    {
        //Declarations of variables
        var userId = "UserId";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1 };
        var jobRole = new JobRole() { Id = 1, Name = "JobRoleTest", JobRoleCategoryId = jobRoleCategory.Id, CompanyId = company.Id };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRoleCategory);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AnyAsync(
            It.IsAny<GetJobroleByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AddAsync(
            It.IsAny<JobRole>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        var result = await jobRoleService.CreateCustomJobRoleAsync(jobRole, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobRoleDuplicated.Replace("[name]", jobRole.Name));
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurWithCreateJobRole_ReturnError()
    {
        //Declarations of variables
        var userId = "UserId";
        var testError = "Error to create a JobRole";
        var company = new Company() { Id = 1 };
        var jobRoleCategory = new JobRoleCategory() { Id = 1 };
        var jobRole = new JobRole() { Id = 1, Name = "JobRoleTest", JobRoleCategoryId = jobRoleCategory.Id, CompanyId = company.Id };

        //Create the simulated service
        var jobRoleService = ServiceUtilities.CreateJobRoleService(
            out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out _
            );

        //Configurations for tests
        jobRoleCategoryRepository.Setup(jobRoleCategoryRepository => jobRoleCategoryRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRoleCategory);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AnyAsync(
            It.IsAny<GetJobroleByNameSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(false);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.AddAsync(
            It.IsAny<JobRole>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await jobRoleService.CreateCustomJobRoleAsync(jobRole, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
