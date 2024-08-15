using Ardalis.Result;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.CDN;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Interfaces.Projects;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.JobRoles;
using NetForemost.Core.Specifications.ProjectCompanyUser;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Projects;
public class ProjectService : IProjectService
{
    private readonly IAsyncRepository<Company> _companyRepository;
    private readonly IAsyncRepository<CompanyUser> _companyUserRepository;
    private readonly IAsyncRepository<Project> _projectRepository;
    private readonly IAsyncRepository<ProjectCompanyUser> _projectCompanyUserRepository;
    private readonly IAsyncRepository<JobRole> _jobRoleRepository;
    private readonly IAsyncRepository<ProjectAvatar> _projectAvatarRepository;
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    private readonly string bucketDomainName = "https://storage.googleapis.com";

    public ProjectService(IAsyncRepository<Company> companyRepository, IAsyncRepository<CompanyUser> companyUserRepository, IAsyncRepository<Project> projectRepository
        , IAsyncRepository<ProjectCompanyUser> projectCompanyUserRepository, IAsyncRepository<JobRole> jobRoleRepository, IAsyncRepository<ProjectAvatar> projectAvatarRepository, StorageClient storageClient, IOptions<CloudStorageConfig> config)
    {
        _companyRepository = companyRepository;
        _companyUserRepository = companyUserRepository;
        _projectRepository = projectRepository;
        _projectCompanyUserRepository = projectCompanyUserRepository;
        _jobRoleRepository = jobRoleRepository;
        _projectAvatarRepository = projectAvatarRepository;
        _storageClient = storageClient;
        _bucketName = config.Value.BucketName;
    }

    public async Task<Result<Project>> CreateProjectAsync(Project project, string userId)
    {
        try
        {
            //Verify If  Company exist
            var existCompany = await _companyRepository.AnyAsync(project.CompanyId);

            if (!existCompany)
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyNotFound
                    }
                    });

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            //verifying that the end date is greater than the starting date
            DateTime emptyDateTime = DateTime.MinValue;
            if (project.EndEstimatedDate != emptyDateTime && project.StartedDate != emptyDateTime)
            {
                if (project.EndEstimatedDate <= project.StartedDate)
                {
                    return Result<Project>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.ProjectDatesIncorrect
                            }
                        }
                    );
                }
            }

            //verifying that not exist another poject with the same name
            var projectNameDuplicated = await _projectRepository.AnyAsync(new GetProjectsByNameSpecification(project.Name, project.CompanyId));

            if (projectNameDuplicated)
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.ProjectNameDuplicated.Replace("[name]",project.Name)
                    }
                    });

            //Set default values
            project.CreatedAt = DateTime.UtcNow;
            project.CreatedBy = userId;
            project.TechStack = new string[] { };

            var projectCreated = await _projectRepository.AddAsync(project);

            return Result<Project>.Success(projectCreated);

        }
        catch (Exception ex)
        {
            return Result<Project>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
    public async Task<Result<Project>> UpdateProjectAsync(Project project, string userId)
    {
        try
        {

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            //verify project exist
            var existProject = await _projectRepository.GetByIdAsync(project.Id);

            if (existProject is null)
            {
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.ProjectNotFound.Replace("[id]",project.Id.ToString())
                    }
                });
            }

            //verify comapny exist
            var existCompany = await _companyRepository.GetByIdAsync(project.CompanyId);
            
            if (existCompany is null)
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyNotFound
                    }
                    });

            //verifying that the end date is greater than the starting date
            DateTime emptyDateTime = new();
            if (project.EndEstimatedDate != emptyDateTime && project.StartedDate != emptyDateTime)
            {
                if (project.EndEstimatedDate <= project.StartedDate)
                {
                    return Result<Project>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.ProjectDatesIncorrect
                            }
                        }
                    );
                }
            }

            //verifying that not exist another poject with the same name
            var projectNameDuplicated = await _projectRepository.AnyAsync(new GetProjectsByNameSpecification(project.Name, project.CompanyId));

            if (projectNameDuplicated)
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.ProjectNameDuplicated.Replace("[name]",project.Name)
                    }
                    });

            // Since we needed to update the endpoint: https://netforemost.atlassian.net/browse/NFCA-339
            string[] emptyProjectStack = { };

            //Set default values
            existProject.AddUpdateInfo(userId);

            //set other values
            existProject.Description = project.Description;
            existProject.Name = project.Name;
            existProject.CompanyId = project.CompanyId;
            existProject.TechStack = project.TechStack != null ? project.TechStack : emptyProjectStack;
            existProject.Budget = project.Budget;
            existProject.StartedDate = project.StartedDate;
            existProject.EndEstimatedDate = project.EndEstimatedDate;
            existProject.ProjectImageUrl = project.ProjectImageUrl;

            await _projectRepository.UpdateAsync(existProject);

            //Add navegation entities
            existProject.Company = existCompany;

            return Result<Project>.Success(existProject);
        }
        catch (Exception ex)
        {
            return Result<Project>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<ProjectStatusDto>>> FindProjectsAsync(
            int projectId, string name, string description,
            int companyId, string[] techStack,
            float BudgetRangeFrom, float BudgetRangeTo,
            string userId, DateTime? dateStartTo, DateTime? dateStartFrom,
            DateTime? dateEndTo, DateTime? dateEndFrom, string userIdFilter,
            int pageNumber = 1, int perPage = 10
        )
    {
        try
        {
            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(companyId, userId));

            if (!userBelongToCompany)
                return Result<PaginatedRecord<ProjectStatusDto>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                 });

            //count the records
            var count = await _projectRepository.CountAsync(
                new GetProjectSpecification(
                    userIdFilter,
                    projectId,
                    name, description, companyId,
                    techStack, BudgetRangeFrom,
                    BudgetRangeTo, dateStartTo,
                    dateStartFrom, dateEndTo,
                    dateEndFrom, pageNumber, perPage, false
                    ));

            //the records of project
            List<ProjectStatusDto> projects = await _projectRepository.ListAsync(
                new GetProjectSpecification(
                    userIdFilter,
                    projectId,
                    name, description, companyId,
                    techStack, BudgetRangeFrom,
                    BudgetRangeTo, dateStartTo,
                    dateStartFrom, dateEndTo,
                    dateEndFrom, pageNumber, perPage, true
                    ));

            // Paginate result
            var paginatedRecords = new PaginatedRecord<ProjectStatusDto>(projects, count, perPage, pageNumber);

            return Result<PaginatedRecord<ProjectStatusDto>>.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result<PaginatedRecord<ProjectStatusDto>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<ProjectCompanyUser>> CreateProjectCompanyUserAsync(ProjectCompanyUser projectCompanyUser, string userId)
    {
        try
        {
            //validate company User
            var companyUser = await _companyUserRepository.GetByIdAsync(projectCompanyUser.CompanyUserId);

            if (companyUser is null)
                return Result<ProjectCompanyUser>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyUserNotFound
                    }
                    }
                );

            //validate job role
            var isValidJobRole = await _jobRoleRepository.AnyAsync(new GetJobRoleByCompanyIdSpecification(projectCompanyUser.JobRoleId, companyUser.CompanyId));

            if (!isValidJobRole)
                return Result<ProjectCompanyUser>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.JobRoleIdNotFound.Replace("[id]",projectCompanyUser.JobRoleId.ToString())
                    }
                    }
                );

            //validate exist project
            var existProject = await _projectRepository.GetByIdAsync(projectCompanyUser.ProjectId);

            if (existProject is null)
                return Result<ProjectCompanyUser>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.ProjectNotFound
                    }
                    }
                );

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(existProject.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<ProjectCompanyUser>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            //validate record duplicated
            var isRecordDuplicated = await _projectCompanyUserRepository.AnyAsync(new GetProjectCompanyUserByCompanyUserIdSpecification(projectCompanyUser.ProjectId, projectCompanyUser.CompanyUserId));

            if (isRecordDuplicated)
                return Result<ProjectCompanyUser>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.ProjectCompanyUserDuplicated.Replace("[id]", projectCompanyUser.CompanyUserId.ToString())
                    }
                    }
                );

            //Set default values
            projectCompanyUser.CreatedAt = DateTime.UtcNow;
            projectCompanyUser.CreatedBy = userId;

            await _projectCompanyUserRepository.AddAsync(projectCompanyUser);

            return Result.Success(projectCompanyUser);
        }
        catch (Exception ex)
        {
            return Result<ProjectCompanyUser>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<ProjectCompanyUser>> UpdateProjectCompanyUserStatusAsync(int projectId, int companyuserId, string userId)
    {
        try
        {
            var userCompany = await _projectCompanyUserRepository.FirstOrDefaultAsync(new GetProjectCompanyUserByCompanyUserIdSpecification(projectId, companyuserId));

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(userCompany.Project.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<ProjectCompanyUser>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            //Enabling or disabling the user
            userCompany.IsActive = !userCompany.IsActive;

            //set value default
            userCompany.UpdatedAt = DateTime.UtcNow;
            userCompany.UpdatedBy = userId;

            await _projectCompanyUserRepository.UpdateAsync(userCompany);

            return Result.Success(userCompany);
        }
        catch (Exception ex)
        {
            return Result<ProjectCompanyUser>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<Project>>> FindProjectsByCompanyIdAsync(int companyId, string userId)
    {
        try
        {

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(companyId, userId));

            if (!userBelongToCompany)
                return Result<List<Project>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            // Verify that company exist
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company == null)
            {
                return Result<List<Project>>.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.CompanyNotFound
                    }
                });
            }

            // Get all projects by company
            var projects = await _projectRepository.ListAsync(new GetProjectsByCompanyIdSpecification(companyId));

            return Result<List<Project>>.Success(projects);

        }
        catch (Exception ex)
        {
            return Result<List<Project>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<UsersByCompanyDto>>> GetUsersByCompanyId(int companyId, int projectId, int perPage, int pageNumber)
    {
        try
        {

            // Verify if company exist
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null)
            {
                return Result<PaginatedRecord<UsersByCompanyDto>>.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.CompanyNotFound
                    }
                });
            }

            // Verify if project exist
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project is null)
            {
                return Result<PaginatedRecord<UsersByCompanyDto>>.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.ProjectNotFound
                    }
                });
            }

            List<UsersByCompanyDto> usersByCompany = await _companyUserRepository
                .ListAsync(new GetUsersByCompanyIdSpecification(companyId));
            int countRecords = usersByCompany.Count();
            List<ProjectCompanyUser> projectCompanyUsersByProjectId = await _projectCompanyUserRepository
                .ListAsync(new GetProjectsByCompanyUserSpecification(projectId));

            var usersProjectsByCompany = usersByCompany.Select(user => new UsersByCompanyDto
            {
                CompanyUserId = user.CompanyUserId,
                FullName = user.FullName,
                CompanyId = user.CompanyId,
                BelongProject = projectCompanyUsersByProjectId.Any(x => x.CompanyUserId == user.CompanyUserId && x.IsActive)
            }).ToList();

            var paginatedRecords = new PaginatedRecord<UsersByCompanyDto>(usersProjectsByCompany, countRecords, perPage, pageNumber);

            return Result<PaginatedRecord<UsersByCompanyDto>>.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
    public async Task<Result<Project>> DeleteProject(string userId, int projectId)
    {
        try
        {
            //Check if companyUser exist
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project is null || project.isDeleted)
                return Result<Project>.Invalid(new List<ValidationError>
                {
                        new() { ErrorMessage = ErrorStrings.ProjectNotFound }
                });

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<Project>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            //Set update values 
            project.AddDeleteInfo(userId);

            //Update in database
            await _projectRepository.UpdateAsync(project);

            return Result<Project>.Success(project);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<ProjectAvatar>>> AddAvatarToProjectAsync(string? Name, string? Description, int projectId, List<IFormFile> AvatarImages, string userId)
    {
        try
        {
            //Verify If  Company exist
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project is null)
                return Result<List<ProjectAvatar>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.ProjectNotFound.Replace("[id]",projectId.ToString())
                    }
                    });

            //verify that the user belong to the company
            var userBelongsToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));
            if (!userBelongsToCompany)
                return Result<List<ProjectAvatar>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            // URL List
            var avatars = new List<ProjectAvatar>();

            // Verify if image is null or empty
            if (AvatarImages is null)
            {
                return Result.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.ImageFileEmpty
                        }
                    });
            }

            foreach (var file in AvatarImages)
            {
                // Validate image file
                if (file is null || file.Length == 0)
                {
                    return Result.Invalid(
                        new()
                        {
                            new()
                            {
                                ErrorMessage = ErrorStrings.ImageFileEmpty
                            }
                        });
                }

                // Get FileName
                var fileName = Path.GetFileName(file.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var objectName = Guid.NewGuid().ToString();

                    await _storageClient.UploadObjectAsync(_bucketName, objectName, file.ContentType, memoryStream);

                    // Generate avatars.
                    var newAvatar = new ProjectAvatar()
                    {
                        Name = Name,
                        Description = Description,
                        ProjectId = projectId,
                        AvatarImageUrl = ($"{bucketDomainName}/{_bucketName}/{objectName}")
                    };
                    newAvatar.AddCreatedInfo(userId);
                    avatars.Add(newAvatar);
                }
            }


            //update-database
            await _projectAvatarRepository.AddRangeAsync(avatars);

            return Result<List<ProjectAvatar>>.Success(avatars);

        }
        catch (Exception ex)
        {
            return Result<List<ProjectAvatar>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Project>> ArchiveProjectAsync(string userId, int projectId)
    {
        try
        {
            //Check if companyUser exist
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project is null)
                return Result<Project>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = ErrorStrings.ProjectNotFound.Replace("[id]", projectId.ToString()) }
                });

            if (project.IsArchived)
                return Result<Project>.Error(ErrorStrings.RegisterAlreadyArchived);

            //verify that the user belongs to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<Project>.Invalid(new List<ValidationError>(){
                        new()
                        {
                            ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                        }
                    }
                );

            //Set update values            
            project.AddArchiveInfo(userId);

            //Update in database
            await _projectRepository.UpdateAsync(project);

            return Result<Project>.Success(project);
        }
        catch (Exception ex)
        {
            return Result<Project>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Project>> UnarchiveProjectAsync(string userId, int projectId)
    {
        try
        {
            //Check if companyUser exist
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project is null)
                return Result<Project>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = ErrorStrings.ProjectNotFound.Replace("[id]", projectId.ToString()) }
                });

            if (!project.IsArchived)
                return Result<Project>.Error(ErrorStrings.RegisterAlreadyUnarchived);

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<Project>.Invalid(new List<ValidationError>(){
                        new()
                        {
                            ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                        }
                        }
                );

            //Set update values            
            project.RemoveArchivedInfo();

            //Update in database
            await _projectRepository.UpdateAsync(project);

            return Result<Project>.Success(project);
        }
        catch (Exception ex)
        {
            return Result<Project>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<ProjectAvatar>>> GetProjectAvatarsAsync(int projectId)
    {
        try
        {
            var avatars = await _projectAvatarRepository.ListAsync(new GetProjectAvatarByProjectIdSpecification(projectId));

            return Result<List<ProjectAvatar>>.Success(avatars);
        }
        catch (Exception ex)
        {
            return Result<List<ProjectAvatar>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<ProjectCompanyUser>>> AddProjectCompanyUsersList(int companyUserId, int[] projectListIds, string userId)
    {
        try
        {
            //Check if companyUser exist
            var companyUser = await _companyUserRepository.GetByIdAsync(companyUserId);

            if (companyUser is null)
                return Result<List<ProjectCompanyUser>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyUserNotFound
                    }
                    });

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(companyUser.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<List<ProjectCompanyUser>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            //Search projectCompanyUsers
            var projects = await _projectCompanyUserRepository.ListAsync(new GetProjectCompanyUserByCompanyUserId(companyUserId, projectListIds));

            //Search projectCompanyUsers
            List<ProjectCompanyUser> resultList = new List<ProjectCompanyUser>();

            foreach (var projectId in projectListIds)
            {
                var projectDetails = projects.Find(el => el.ProjectId == projectId);
                if (projectDetails is not null)
                    continue;

                //Check if project exist
                var project = await _projectRepository.GetByIdAsync(projectId);

                if (project is null)
                    return Result<List<ProjectCompanyUser>>.Invalid(new List<ValidationError>(){
                        new()
                        {
                            ErrorMessage=ErrorStrings.ProjectNotFound.Replace("[id]",projectId.ToString())
                        }
                        });

                //Check if project is property of the Company
                if (project.CompanyId != companyUser.CompanyId)
                    return Result<List<ProjectCompanyUser>>.Invalid(new List<ValidationError>(){
                        new()
                        {
                            ErrorMessage=ErrorStrings.ProjectNotOwnedTheCompany
                        }
                        });

                //Create new project Company Users
                var newProjectCompanyUser = new ProjectCompanyUser()
                {
                    ProjectId = project.Id,
                    CompanyUserId = companyUser.Id,
                    JobRoleId = (int)companyUser.JobRoleId,
                    IsActive = true
                };
                newProjectCompanyUser.AddCreatedInfo(userId);
                resultList.Add(newProjectCompanyUser);
            }

            //update database
            await _projectCompanyUserRepository.AddRangeAsync(resultList);

            return Result<List<ProjectCompanyUser>>.Success(resultList);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));

        }
    }

    public async Task<Result<List<ProjectCompanyUser>>> UpdateProjectCompanyUsersListActiveStatus(int companyUserId, int[] projectListIds, string userId)
    {
        try
        {
            //Check if companyUser exist
            var companyUser = await _companyUserRepository.GetByIdAsync(companyUserId);

            if (companyUser is null)
                return Result<List<ProjectCompanyUser>>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyUserNotFound
                    }
                    });

            //verify that the user belong to the company
            var userBelongToCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(companyUser.CompanyId, userId));

            if (!userBelongToCompany)
                return Result<List<ProjectCompanyUser>>.Invalid(new List<ValidationError>(){

                    new()
                    {
                        ErrorMessage=ErrorStrings.UserDoesNotBelongToTheCompany
                    }
                    });

            //Search projectCompanyUsers
            var projects = await _projectCompanyUserRepository.ListAsync(new GetProjectCompanyUserByCompanyUserId(companyUserId, projectListIds));

            foreach (var projectId in projectListIds)
            {
                var project = projects.Find(el => el.ProjectId == projectId);
                if (project is not null)
                {
                    project.IsActive = !project.IsActive;
                    project.AddUpdateInfo(userId);
                }
            }

            //update database
            if (projects is not null)
                await _projectCompanyUserRepository.UpdateRangeAsync(projects);

            return Result<List<ProjectCompanyUser>>.Success(projects);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<ProjectCompanyUser>>> AddProjectCompanyUsersListByCompanyUserIds(int projectId, int[] companyUserIdsList, string userId)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project is null)
                return Result<List<ProjectCompanyUser>>.Invalid(ErrorHelper.Error(ErrorStrings.ProjectNotFound));

            bool isUserInCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));
            if (!isUserInCompany)
                return Result<List<ProjectCompanyUser>>.Invalid(ErrorHelper.Error(ErrorStrings.UserDoesNotBelongToTheCompany));

            var existingCompanyUsers = await _companyUserRepository.ListAsync(new GetCompanyUsersByCompanyIdSpecification(project.CompanyId, companyUserIdsList));
            var nonExistingIds = companyUserIdsList.Except(existingCompanyUsers.Select(cu => cu.Id)).ToList();
            if (nonExistingIds.Any())
                return Result<List<ProjectCompanyUser>>.Invalid(ErrorHelper.Error(ErrorStrings.CompanyUserNotFound, nonExistingIds));

            var existingProjectCompanyUsers = await _projectCompanyUserRepository.ListAsync(new GetProjectCompanyUserByProjectId(projectId, companyUserIdsList));

            // Check for existing IDs in projectCompanyUser
            var existingIdsInProjectCompanyUser = existingProjectCompanyUsers.Select(pc => pc.CompanyUserId).Intersect(companyUserIdsList).ToList();
            if (existingIdsInProjectCompanyUser.Any())
            {
                Console.Error.WriteLine(ErrorStrings.ProjectCompanyUserDuplicatedWithId.Replace("[id]", string.Join(", ", existingIdsInProjectCompanyUser)));
            }

            var newProjectCompanyUsers = existingCompanyUsers
                .Where(cu => !existingProjectCompanyUsers.Any(pc => pc.CompanyUserId == cu.Id))
                .Select(cu => CreateProjectCompanyUser(project.Id, cu, userId))
                .ToList();

            await _projectCompanyUserRepository.AddRangeAsync(newProjectCompanyUsers);
            return Result<List<ProjectCompanyUser>>.Success(newProjectCompanyUsers);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<NoContentResult>> UpdateCompanyUsersStatusListForProject(int projectId, int[] companyUserIdsList, string userId)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project is null)
                return Result<NoContentResult>.Invalid(ErrorHelper.Error(ErrorStrings.ProjectNotFound));

            var isUserInCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));
            if (!isUserInCompany)
                return Result<NoContentResult>.Invalid(ErrorHelper.Error(ErrorStrings.UserDoesNotBelongToTheCompany));

            var projectCompanyUsers = await _projectCompanyUserRepository.ListAsync(new GetProjectCompanyUserByProjectId(projectId, companyUserIdsList));
            if (projectCompanyUsers == null || !projectCompanyUsers.Any())
                return Result<NoContentResult>.Invalid(
                    ErrorHelper.Error(
                        ErrorStrings.ProjectCompanyUserNotFoundWithId.Replace(
                            "[id]", string.Join(", ", companyUserIdsList)
                        )
                    )
                );

            foreach (int companyId in companyUserIdsList)
            {
                var projectCompanyUser = projectCompanyUsers.FirstOrDefault(pc => pc.CompanyUserId == companyId);
                if (projectCompanyUser != null)
                {
                    projectCompanyUser.IsActive = !projectCompanyUser.IsActive;
                    projectCompanyUser.AddUpdateInfo(userId);
                }
            }

            await _projectCompanyUserRepository.UpdateRangeAsync(projectCompanyUsers);

            return Result<NoContentResult>.Success(new NoContentResult());
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<NoContentResult>> PatchProjectAsync(int projectId, JsonPatchDocument<Project> patchDocument, string userId)
    {
        Result<Project> projectResult = await _projectRepository.GetByIdAsync(projectId);
        if (!projectResult.IsSuccess)
            return Result<NoContentResult>.Invalid(ErrorHelper.Error(ErrorStrings.ProjectNotFound));

        Project project = projectResult.Value;
        var isUserInCompany = await _companyUserRepository.AnyAsync(new CheckUserIsInCompanySpecification(project.CompanyId, userId));
        if (!isUserInCompany)
            return Result<NoContentResult>.Invalid(ErrorHelper.Error(ErrorStrings.UserDoesNotBelongToTheCompany));

        Result patchValidationResult = PatchValidationHelper.ValidatePatchDocument(patchDocument);

        if (!patchValidationResult.IsSuccess)
        {
            return Result<NoContentResult>.Error(patchValidationResult.Errors.ToArray());
        }

        patchDocument.ApplyTo(project);
        await _projectRepository.SaveChangesAsync();

        return Result<NoContentResult>.Success(new NoContentResult());
    }

    #region Private methods

    private ProjectCompanyUser CreateProjectCompanyUser(int projectId, CompanyUser companyUser, string userId)
    {
        var projectCompanyUser = new ProjectCompanyUser
        {
            ProjectId = projectId,
            CompanyUserId = companyUser.Id,
            JobRoleId = (int)companyUser.JobRoleId,
            IsActive = true
        };
        projectCompanyUser.AddCreatedInfo(userId);
        return projectCompanyUser;
    }

    #endregion
}
