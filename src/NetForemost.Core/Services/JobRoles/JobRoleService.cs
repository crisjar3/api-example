using Ardalis.Result;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Interfaces.JobRoles;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.JobRoles;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using System.ComponentModel.Design;

namespace NetForemost.Core.Services.JobRoles;

public class JobRoleService : IJobRoleService
{
    private readonly IAsyncRepository<JobRoleCategory> _jobRoleCategoryRepository;
    private readonly IAsyncRepository<Company> _companyRepository;
    private readonly IAsyncRepository<JobRole> _jobRoleRepository;
    private readonly IAsyncRepository<CompanyUser> _companyUserRepository;

    public JobRoleService(IAsyncRepository<JobRoleCategory> jobRoleCategoryRepository, IAsyncRepository<Company> companyRepository, IAsyncRepository<JobRole> jobRoleRepository, IAsyncRepository<CompanyUser> companyUserRepository)
    {
        _jobRoleCategoryRepository = jobRoleCategoryRepository;
        _companyRepository = companyRepository;
        _jobRoleRepository = jobRoleRepository;
        _companyUserRepository = companyUserRepository;
    }

    public async Task<Result<JobRole>> CreateCustomJobRoleAsync(JobRole jobRole, string userId)
    {
        try
        {
            //verify if exist companyId
            var existCompany = await _companyRepository.GetByIdAsync(jobRole.CompanyId);
            
            if (existCompany is null)
                return Result<JobRole>.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.CompanyNotFound
                    }
                });

            //verify if exist jobRoleCategory
            var existJobRoleCategory = await _jobRoleCategoryRepository.GetByIdAsync(jobRole.JobRoleCategoryId);

            if (existJobRoleCategory is null)
                return Result<JobRole>.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.JobRoleCategoryNotFound.Replace("[id]", jobRole.JobRoleCategoryId.ToString())
                        }
                    });

            //veirfy not exist two record with the same name
            var someHasTheSameName = await _jobRoleRepository.AnyAsync(new GetJobroleByNameSpecification(jobRole.Name, jobRole.CompanyId));

            if (someHasTheSameName)
                return Result<JobRole>.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.JobRoleDuplicated.Replace("[name]", jobRole.Name)
                        }
                    });

            //Set default values
            jobRole.CreatedAt = DateTime.UtcNow;
            jobRole.CreatedBy = userId;

            var jobRoleCreated = await _jobRoleRepository.AddAsync(jobRole);

            //Add navegation entities
            jobRole.Company = existCompany;
            jobRole.JobRoleCategory = existJobRoleCategory;

            return Result<JobRole>.Success(jobRoleCreated);
        }
        catch (Exception ex)
        {
            return Result<JobRole>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<JobRoleCategory>> CreateCustomJobRoleCategoryAsync(JobRoleCategory jobRoleCategory, string userId)
    {
        try
        {
            //verify if exist companyId
            var existCompany = await _companyRepository.GetByIdAsync(jobRoleCategory.CompanyId);

            if (existCompany is null)
                return Result<JobRoleCategory>.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.CompanyNotFound
                    }
                });

            //verify not exist two record with the same name
            var someHasTheSameName = await _jobRoleCategoryRepository.AnyAsync(new GetJobRoleCategoriesByNameSpecification(jobRoleCategory.Name, jobRoleCategory.CompanyId));

            if (someHasTheSameName)
                return Result<JobRoleCategory>.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.JobRoleCategoryDuplicated.Replace("[name]", jobRoleCategory.Name)
                        }
                    });

            //Set default values
            jobRoleCategory.CreatedAt = DateTime.UtcNow;
            jobRoleCategory.CreatedBy = userId;

            var jobRoleCategoryCreated = await _jobRoleCategoryRepository.AddAsync(jobRoleCategory);

            //Add navegation entities
            jobRoleCategory.Company = existCompany;

            return Result<JobRoleCategory>.Success(jobRoleCategoryCreated);
        }
        catch (Exception ex)
        {
            return Result<JobRoleCategory>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<JobRoleCategory>>> FindJobRoleCategoriesAsync(int CompanyId)
    {
        try
        {
            //verify if exist companyId
            if (CompanyId is not 0)
            {
                var company = await _companyRepository.GetByIdAsync(CompanyId);

                if (company is null)
                    return Result<List<JobRoleCategory>>.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyNotFound
                    }
                });
            }

            var jobRoleCategories = await _jobRoleCategoryRepository.ListAsync(new GetJobRoleCatgoriesWithJobRolesSpecification(CompanyId));

            return Result<List<JobRoleCategory>>.Success(jobRoleCategories);
        }
        catch (Exception ex)
        {
            return Result<List<JobRoleCategory>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<JobRole>>> GetJobRolesAsync(int companyId,string userId)
    {
        try
        {
            //Verify If  Company exist
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null)
                return Result<List<JobRole>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyNotFound
                    }
                    });

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(companyId, userId));

            if (!userBelongToCompany)
                return Result<List<JobRole>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            var jobRoles = await _jobRoleRepository.ListAsync(new GetJobRolesByCompanyIdSpecification(companyId));

            return Result<List<JobRole>>.Success(jobRoles);
        }
        catch (Exception ex)
        {
            return Result<List<JobRole>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}