using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Specifications.Companies;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Companies;

public class CompanyUserService : ICompanyUserService
{

    private readonly IAsyncRepository<Company> _companyRepository;
    private readonly IAsyncRepository<Entities.TimeZones.TimeZone> _timeZoneRepository;
    private readonly IAsyncRepository<CompanyUser> _companyUserRepository;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAsyncRepository<JobRole> _jobRoleRepository;
    private readonly IAsyncRepository<ProjectCompanyUser> _projectCompanyUserRepository;
    private readonly IAsyncRepository<Project> _projectRepository;

    public CompanyUserService(
        IAsyncRepository<Company> companyRepository,
        IAsyncRepository<Entities.TimeZones.TimeZone> timeZoneRepository,
        IAsyncRepository<CompanyUser> companyUserRepository,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IAsyncRepository<JobRole> jobRoleRepository,
        IAsyncRepository<ProjectCompanyUser> projectCompanyUserRepository,
        IAsyncRepository<Project> projectRepository
    )
    {
        _companyRepository = companyRepository;
        _timeZoneRepository = timeZoneRepository;
        _companyUserRepository = companyUserRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _jobRoleRepository = jobRoleRepository;
        _projectCompanyUserRepository = projectCompanyUserRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<CompanyUser>> CreateTeamMemberAsync(CompanyUser newCompanyUser, string userId)
    {
        try
        {
            //Check if user to add exist
            var user = await _userManager.FindByIdAsync(newCompanyUser.UserId);

            if (user is null) return Result<CompanyUser>.Invalid(new() { new() { ErrorMessage = ErrorStrings.User_NotFound } });

            //Check if user to add is active
            if (!user.IsActive) return Result<CompanyUser>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserNotAvailableToAddInTeam } });

            //Check if company exist
            var company = await _companyRepository.GetByIdAsync(newCompanyUser.CompanyId);

            if (company is null) return Result<CompanyUser>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CompanyNotFound } });

            //Check if the user to create records belongs to the company
            var existCompanyUser = await _companyUserRepository.ListAsync(new CheckUserIsInCompanySpecification(newCompanyUser.CompanyId, userId));

            if (!existCompanyUser.Any()) return Result<CompanyUser>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserDoesNotBelongToTheCompany } });

            //Check if user to add is exist in the company
            var existCompanyUserToAdd = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(newCompanyUser.CompanyId, newCompanyUser.UserId));

            if (existCompanyUserToAdd) return Result<CompanyUser>.Invalid(new() { new() { ErrorMessage = ErrorStrings.TalentExistsInTheCompany } });

            //Check if role exist
            var role = await _roleManager.FindByIdAsync(newCompanyUser.RoleId);
            
            if (role is null)
            {
                return Result.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.RoleNotFound
                    }
                });
            }
            
            //Check if timezone Exist
            var timeZone = await _timeZoneRepository.GetByIdAsync(newCompanyUser.TimeZoneId);

            if (timeZone is null)
            {
                return Result.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.TimeZoneNotExist
                    }
                });
            }
            
            //Check if JobRoleExist
            var jobRole = await _jobRoleRepository.GetByIdAsync(newCompanyUser.JobRoleId);
            if (jobRole is null)
            {
                return Result.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.JobRoleIdNotFound
                    }
                });
            }
            
            var companyUser = new CompanyUser()
            {
                UserName = newCompanyUser.UserName,
                TimeZoneId = newCompanyUser.TimeZoneId,
                JobRoleId = newCompanyUser.JobRoleId,
                CompanyId = newCompanyUser.CompanyId,
                RoleId = role.Id,
                UserId = newCompanyUser.UserId,
                IsActive = true,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };
            
            companyUser = await _companyUserRepository.AddAsync(companyUser);
            
            //Add navegation entities
            companyUser.TimeZone = timeZone;
            companyUser.JobRole = jobRole;
            companyUser.Company = company;
            companyUser.Role = role;
            companyUser.User = user;
            Result result = await CreateProjectCompanyUserForProjectsAccesibleForEveryone(companyUser.Id, (int)companyUser.JobRoleId);
            
            if (!result.IsSuccess)
            {
                Console.Error.WriteLine(result.Errors.First().ToString());
            }
            
            return Result<CompanyUser>.Success(companyUser);
        }
        catch (Exception ex)
        {
            return Result<CompanyUser>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<UserSettingCompanyUserDto>>> FindTeamMembersAsync(int companyId, int[] TimeZoneIds, int[] companyUserIds, bool isArchived, DateTime from, DateTime to, int pageNumber, int perPage, string userId)
    {
        try
        {
            //Verify if user to make a request is belong to company
            var existUserInCompany = await VerifyUserCompanyAsync(userId, companyId);

            if (!existUserInCompany.IsSuccess)
                return Result.Invalid(new() { new() { ErrorMessage = existUserInCompany.ValidationErrors.FirstOrDefault().ErrorMessage } });

            //Get all records paginated
            var usersCompany = await _companyUserRepository.ListAsync(new GetUsersByCompanySpecification(companyId, TimeZoneIds, companyUserIds, isArchived, from, to,
                pageNumber, perPage, true));

            //Count total records from query result without paginate
            int count = await _companyUserRepository.CountAsync(new GetUsersByCompanySpecification(companyId, TimeZoneIds, companyUserIds, isArchived, from, to,
                pageNumber, perPage, false));
            //Paginate result
            var paginatedRecords = new PaginatedRecord<UserSettingCompanyUserDto>(usersCompany, count, perPage, pageNumber);

            return Result.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<CompanyUser>> UpdateCompanyUserAsync(CompanyUser newDataCompanyUser, string userId)
    {
        try
        {
            //Verify if companyUser to make a request exist
            var companyUser = await _companyUserRepository.GetByIdAsync(newDataCompanyUser.Id);

            if (companyUser is null)
                return Result<CompanyUser>.Invalid(new List<ValidationError>()
                {
                    new() { ErrorMessage = ErrorStrings.CompanyUserNotFound }
                });

            //Check if the user to update records belongs to the company
            var existUserInCompany = await VerifyUserCompanyAsync(userId, companyUser.CompanyId);

            if (!existUserInCompany.IsSuccess)
                return Result<CompanyUser>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = ErrorStrings.UserDoesNotBelongToTheCompany }
                });

            //Verify if TimeZone exist
            var timeZone = await _timeZoneRepository.GetByIdAsync(newDataCompanyUser.TimeZoneId);

            if (timeZone is null)
                return Result<CompanyUser>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage=ErrorStrings.TimeZoneNotExist }
                });

            //validate role
            var role = await _roleManager.FindByIdAsync(newDataCompanyUser.RoleId);

            if (role is null)
                return Result<CompanyUser>.Invalid(new List<ValidationError>
                {
                    new(){ ErrorMessage=ErrorStrings.RoleNotFound.Replace("[id]",newDataCompanyUser.RoleId.ToString()) }
                });

            //validate job role
            var jobRole = await _jobRoleRepository.GetByIdAsync((int)newDataCompanyUser.JobRoleId);

            if (jobRole is null)
                return Result<CompanyUser>.Invalid(new List<ValidationError>
                {
                    new(){ ErrorMessage=ErrorStrings.JobRoleIdNotFound.Replace("[id]",newDataCompanyUser.JobRoleId.ToString()) }
                });

            //Set update values
            companyUser.UpdatedAt = DateTime.UtcNow;
            companyUser.UpdatedBy = userId;
            if (!newDataCompanyUser.UserName.IsNullOrEmpty())
                companyUser.UserName = newDataCompanyUser.UserName;
            companyUser.RoleId = newDataCompanyUser.RoleId;
            companyUser.JobRoleId = newDataCompanyUser.JobRoleId;
            companyUser.TimeZoneId = newDataCompanyUser.TimeZoneId;

            //get projects from the companyUser
            var projectsCompanyUser = await _projectCompanyUserRepository.ListAsync(new GetProjectCompanyUserByCompanyUserId(companyUser.Id, null));
            if (projectsCompanyUser is not null)
            {
                foreach (ProjectCompanyUser projectCompanyUser in projectsCompanyUser)
                {
                    projectCompanyUser.JobRoleId = (int)newDataCompanyUser.JobRoleId;
                }

                //Update in database
                await _projectCompanyUserRepository.UpdateRangeAsync(projectsCompanyUser);
            }

            //Update in database
            await _companyUserRepository.UpdateAsync(companyUser);

            //Add navigation entities
            companyUser.Role = role;
            companyUser.JobRole = jobRole;
            companyUser.TimeZone = timeZone;

            return Result<CompanyUser>.Success(companyUser);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<CompanyUser>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<bool>> VerifyUserCompanyAsync(string userId, int companyId)
    {
        try
        {
            //Check if the user to create records belongs to the company
            var existCompanyUser = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(companyId, userId));

            if (!existCompanyUser) return Result<bool>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserDoesNotBelongToTheCompany } });

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<CompanyUser>> GetCompanyUserDetailsAsync(string userId, int companyUserId)
    {
        try
        {
            var companyUser = await _companyUserRepository.FirstOrDefaultAsync(new GetCompanyUserByCompanyUserIdSpecification(companyUserId));

            if (companyUser is null)
                return Result<CompanyUser>.Invalid(new List<ValidationError>()
                {
                    new() { ErrorMessage = ErrorStrings.CompanyUserNotFound }
                });
            
            //Check if the user to get records belongs to the company
            var existUserInCompany = await VerifyUserCompanyAsync(userId, companyUser.CompanyId);

            if (!existUserInCompany.IsSuccess)
                return Result<CompanyUser>.Invalid(new List<ValidationError>()
                {
                    new() { ErrorMessage = existUserInCompany.ValidationErrors.FirstOrDefault().ErrorMessage }
                });
            
            var projects = await _projectRepository.ListAsync(new GetProjectsByCompanyIdSpecification(companyUser.CompanyId));
            
            if (projects is null)
                return Result<CompanyUser>.Success(companyUser);
            
            if (companyUser.ProjectCompanyUsers is null)
                companyUser.ProjectCompanyUsers = new List<ProjectCompanyUser>();
            
            foreach (var project in projects)
            {
                if (companyUser.ProjectCompanyUsers.ToList().FindAll(el => el.ProjectId == project.Id).Count==0)
                {
                    companyUser.ProjectCompanyUsers.Add(new ProjectCompanyUser() { Id = 0, Project = project, ProjectId = project.Id, IsActive = false });
                }
            }
            
            return Result<CompanyUser>.Success(companyUser);
        }
        catch (Exception ex)
        {
            return Result<CompanyUser>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<CompanyUser>> FindCompanyUserAsync(string userId, int companyId)
    {
        try
        {
            //Get companyUser by user id
            var companyUser = await _companyUserRepository.FirstOrDefaultAsync(new GetCompanyUserIdByUserAndCompanySpecification(userId, companyId));

            //Verify if exist the companyUser
            if (companyUser is null)
                return Result<CompanyUser>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = ErrorStrings.CompanyUserNotFound }
                });

            return Result<CompanyUser>.Success(companyUser);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<CompanyUser>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }


    public async Task<Result<PaginatedRecord<CompanyUser>>> GetCompanyUsersAsync(string userId, int companyId, int pageNumber = 1, int perPage = 10)
    {
        try
        {
            //Verify if user to make a request is belong to company
            var existUserInCompany = await VerifyUserCompanyAsync(userId, companyId);

            if (!existUserInCompany.IsSuccess)
                return Result<PaginatedRecord<CompanyUser>>.Invalid(new() { new() { ErrorMessage = existUserInCompany.ValidationErrors.FirstOrDefault().ErrorMessage } });

            //Get all records paginated
            var allCompanyUsers = await _companyUserRepository.ListAsync(new GetCompanyUserByCompanyIdSpecification(companyId, pageNumber, perPage, true));

            //Count total records from query result without paginate
            int countUsers = await _companyUserRepository.CountAsync(new GetCompanyUserByCompanyIdSpecification(companyId, pageNumber, perPage, false));

            //Paginate result
            var paginatedRecords = new PaginatedRecord<CompanyUser>(allCompanyUsers, countUsers, perPage, pageNumber);

            return Result<PaginatedRecord<CompanyUser>>.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result<PaginatedRecord<CompanyUser>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<CompanyUser>> DeleteCompanyUserAsync(string userId, int companyUserId)
    {
        try
        {
            //Check if companyUser exist
            var companyUser = await _companyUserRepository.GetByIdAsync(companyUserId);

            if (companyUser is null || companyUser.isDeleted)
                return Result<CompanyUser>.Invalid(new List<ValidationError>
                {
                        new() { ErrorMessage = ErrorStrings.CompanyUserNotFound }
                });

            //Verify if user to make a request is belong to company
            var existUserInCompany = await VerifyUserCompanyAsync(userId, companyUser.CompanyId);
            if (!existUserInCompany.IsSuccess)
                return Result<CompanyUser>.Invalid(new() { new() { ErrorMessage = existUserInCompany.ValidationErrors.FirstOrDefault().ErrorMessage } });

            //Set update values            
            companyUser.AddDeleteInfo(userId);

            //Update in database
            await _companyUserRepository.UpdateAsync(companyUser);

            return Result<CompanyUser>.Success(companyUser);

        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<CompanyUser>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<CompanyUser>> ArchiveCompanyUserAsync(string userId, int companyUserId)
    {
        //Check if companyUser exist
        var companyUser = await _companyUserRepository.GetByIdAsync(companyUserId);

        if (companyUser is null)
            return Result<CompanyUser>.Invalid(new List<ValidationError>
            {
                new() { ErrorMessage = ErrorStrings.CompanyUserNotFound }
            });

        if (companyUser.IsArchived)
            return Result<CompanyUser>.Error(ErrorStrings.RegisterAlreadyArchived);

        //Verify if user making the request belongs to the current company
        var existUserInCompany = await VerifyUserCompanyAsync(userId, companyUser.CompanyId);

        if (!existUserInCompany.IsSuccess)
            return Result<CompanyUser>.Invalid(new List<ValidationError>()
            {
                new() { ErrorMessage = existUserInCompany.ValidationErrors.FirstOrDefault().ErrorMessage }
            });

        //Set update values            
        companyUser.AddArchiveInfo(userId);

        //Update in database
        await _companyUserRepository.UpdateAsync(companyUser);

        return Result<CompanyUser>.Success(companyUser);
    }

    public async Task<Result<CompanyUser>> UnarchiveCompanyUserAsync(string userId, int companyUserId)
    {
        //Check if companyUser exist
        var companyUser = await _companyUserRepository.GetByIdAsync(companyUserId);

        if (companyUser is null)
            return Result<CompanyUser>.Invalid(new List<ValidationError>
            {
                new() { ErrorMessage = ErrorStrings.CompanyUserNotFound }
            });

        if (!companyUser.IsArchived)
            return Result<CompanyUser>.Error(ErrorStrings.RegisterAlreadyUnarchived);

        //Verify if user making the request belongs to the current company
        var existUserInCompany = await VerifyUserCompanyAsync(userId, companyUser.CompanyId);

        if (!existUserInCompany.IsSuccess)
            return Result<CompanyUser>.Invalid(new List<ValidationError>()
            {
                new() { ErrorMessage = existUserInCompany.ValidationErrors.FirstOrDefault().ErrorMessage }
            });

        //Set update values            
        companyUser.RemoveArchivedInfo();

        //Update in database
        await _companyUserRepository.UpdateAsync(companyUser);

        return Result<CompanyUser>.Success(companyUser);
    }

    public async Task<Result<NoContentResult>> PatchCompanyUserAsync(int companyUserId, JsonPatchDocument<CompanyUser> patchCompanyUser, string userId)
    {
        //Verify if exist companyUser
        var companyUser = await _companyUserRepository.GetByIdAsync(companyUserId);

        if (companyUser is null || companyUser.isDeleted)
        {
            return Result.Invalid(new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = ErrorStrings.CompanyUserNotFound
                }
            });
        }

        //Verify if user making the request belongs to the current company
        var existUserInCompany = await VerifyUserCompanyAsync(userId, companyUser.CompanyId);

        if (!existUserInCompany.IsSuccess)
            return Result.Invalid(new List<ValidationError>()
            {
                new() { ErrorMessage = existUserInCompany.ValidationErrors.FirstOrDefault().ErrorMessage }
            });

        //Verify if patch is valid
        Result patchValidationResult = PatchValidationHelper.ValidatePatchDocument(patchCompanyUser);

        if (!patchValidationResult.IsSuccess)
        {
            return Result.Invalid(new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = patchValidationResult.Errors.FirstOrDefault()
                }
            });
        }

        patchCompanyUser.ApplyTo(companyUser);

        companyUser.AddUpdateInfo(userId);

        await _companyUserRepository.UpdateAsync(companyUser);

        return Result.Success();
    }

    public async Task<Result<PaginatedRecord<GetCompanyArchivedUsersDto>>> GetArchivedCompanyUsersAsync(string userId, int companyId, int pageNumber = 1, int perPage = 10)
    {
        try
        {
            //Verify if user to make a request is belong to company
            var existUserInCompany = await VerifyUserCompanyAsync(userId, companyId);

            if (!existUserInCompany.IsSuccess)
                return Result<PaginatedRecord<GetCompanyArchivedUsersDto>>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserDoesNotBelongToTheCompany } });

            //Get all records paginated
            var allCompanyUsers = await _companyUserRepository.ListAsync(new GetCompanyArchivedUsersByCompanyIdSpecification(companyId, pageNumber, perPage, true));

            //Count total records from query result without paginate
            int countUsers = await _companyUserRepository.CountAsync(new GetCompanyArchivedUsersByCompanyIdSpecification(companyId, pageNumber, perPage, false));

            //Paginate result
            var paginatedRecords = new PaginatedRecord<GetCompanyArchivedUsersDto>(allCompanyUsers, countUsers, perPage, pageNumber);

            return Result<PaginatedRecord<GetCompanyArchivedUsersDto>>.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result<PaginatedRecord<GetCompanyArchivedUsersDto>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    # region Private Methods

    private async Task<Result> CreateProjectCompanyUserForProjectsAccesibleForEveryone(int companyUserId, int companyUserRoleId)
    {
        List<Project> projectsAccesiblesForEveryone = await _projectRepository.ListAsync(
            new GetProjectsAccesiblesForEveryone()
        );

        if (projectsAccesiblesForEveryone.Count == 0)
        {
            return Result.NotFound(new string[] { ErrorStrings.ProjectAccesibleByEveryoneNotFound });
        }

        List<ProjectCompanyUser> projectCompanyUsersList = new();
        foreach (Project project in projectsAccesiblesForEveryone)
        {
            ProjectCompanyUser projectCompanyUser = new ProjectCompanyUser()
            {
                ProjectId = project.Id,
                CompanyUserId = companyUserId,
                JobRoleId = companyUserRoleId,
            };

            projectCompanyUsersList.Add(projectCompanyUser);
        }

        await _projectCompanyUserRepository.AddRangeAsync(projectCompanyUsersList);

        return Result.Success();
    }

    # endregion
}
