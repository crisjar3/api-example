using Ardalis.Result;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Tasks;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Tasks
{
    public class TaskTypeService : ITaskTypeService
    {
        private readonly IAsyncRepository<Company> _companyRepository;
        private readonly IAsyncRepository<CompanyUser> _companyUserRepository;
        private readonly IAsyncRepository<Project> _projectRepository;
        private readonly IAsyncRepository<TaskType> _taskTypeRepository;
        private readonly IAsyncRepository<Entities.Tasks.Task> _taskRepository;


        public TaskTypeService(IAsyncRepository<Company> companyRepository,
                           IAsyncRepository<CompanyUser> companyUserRepository,
                           IAsyncRepository<Project> projectRepository,
                           IAsyncRepository<TaskType> taskTypeRepository,
                           IAsyncRepository<Entities.Tasks.Task> taskRepository)
        {
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _projectRepository = projectRepository;
            _taskTypeRepository = taskTypeRepository;
            _taskRepository = taskRepository;
        }

        public async Task<Result<PaginatedRecord<GetTaskTypesDto>>> GetTaskTypesAsync(
            string userId,
            string name,
            string description,
            int projectId,
            int companyId,
            int pageNumber, int perPage)
        {
            try
            {

                //count the records
                var count = await _taskTypeRepository.CountAsync(
                    new TaskTypesQueryableSpecification(
                        name: name,
                        description: description,
                        projectId: projectId,
                        companyId: companyId,
                        pageNumber: pageNumber,
                        perPage: perPage
                    )
                );

                //the actual records
                var tasks = await _taskTypeRepository.ListAsync(
                    new TaskTypesQueryableSpecification(
                        name: name,
                        description: description,
                        projectId: projectId,
                        companyId: companyId,
                        pageNumber: pageNumber,
                        perPage: perPage
                    )
                );

                // Paginate result
                var paginatedRecords = new PaginatedRecord<GetTaskTypesDto>(tasks, count, perPage, pageNumber);

                return Result.Success(paginatedRecords);
            }
            catch (Exception ex)
            {
                return Result.Error(ErrorHelper.GetExceptionError(ex));
            }
        }


        public async Task<Result<TaskType>> CreateTaskTypeAsync(TaskType taskType, string userId)
        {
            try
            {
                var validatedResult = await ValidateAccessTaskTypeAsync(taskType, userId);
                if (validatedResult.IsSuccess)
                {
                    var namingResult = await ValidateNamingTaskTypeAsync(taskType);
                    if (namingResult.IsSuccess)
                    {
                        //Set default values
                        taskType.CreatedAt = DateTime.UtcNow;
                        taskType.CreatedBy = userId;

                        var taskTypeCreated = await _taskTypeRepository.AddAsync(taskType);

                        return Result<TaskType>.Success(taskTypeCreated);
                    }
                    else
                        return namingResult;

                }
                else
                    return validatedResult;

            }
            catch (Exception ex)
            {
                return Result<TaskType>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        public async Task<Result<TaskType>> UpdateTaskTypeAsync(TaskType taskType, string userId)
        {
            try
            {
                //Get task type from the db
                var dbTaskType = await _taskTypeRepository.GetByIdAsync(taskType.Id);

                if (dbTaskType is null)
                    return Result<TaskType>.Invalid(new List<ValidationError>(){
                        new()
                        {
                            ErrorMessage=ErrorStrings.TaskTypeNotFound.Replace("[id]",taskType.Id.ToString())
                        }
                });

                var accessResult = await ValidateAccessTaskTypeAsync(dbTaskType, userId);
                if (accessResult.IsSuccess)
                {
                    var namingResult = await ValidateNamingTaskTypeAsync(taskType);
                    if (namingResult.IsSuccess)
                    {
                        //Set default values
                        if (!String.IsNullOrEmpty(taskType.Name))
                            dbTaskType.Name = taskType.Name;
                        if (!String.IsNullOrEmpty(taskType.Description))
                            dbTaskType.Description = taskType.Description;
                        dbTaskType.UpdatedAt = DateTime.UtcNow;
                        dbTaskType.UpdatedBy = userId;

                        await _taskTypeRepository.UpdateAsync(dbTaskType);

                        return Result<TaskType>.Success(dbTaskType);
                    }
                    else
                        return namingResult;

                }
                else
                    return accessResult;

            }
            catch (Exception ex)
            {
                return Result<TaskType>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        /// <summary>
        /// Validates that a user has all the access requirements to use a task type
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Result<TaskType>> ValidateAccessTaskTypeAsync(TaskType taskType, string userId)
        {
            try
            {
                taskType.Company = await _companyRepository.GetByIdAsync(taskType.CompanyId);

                if (taskType.Company is null)
                    return Result<TaskType>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.CompanyNotFound
                    }
                    });


                //verify that the user belong to the company
                var companyUser = await _companyUserRepository.FirstOrDefaultAsync(new CheckUserIsInCompanySpecification(taskType.CompanyId, userId));
                if (companyUser is null)
                    return Result<TaskType>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.CompanyUserNotFound
                            }
                        }
                    );


                //validate the project exists and the project belongs to that company
                taskType.Project = await _projectRepository.GetByIdAsync(taskType.ProjectId);

                if (taskType.Project is null)
                    return Result<TaskType>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.ProjectNotFound
                            }
                        }
                    );


                //Validate that the project belongs to the company, and the companyUser have the same companyId
                if (taskType.Project.CompanyId != companyUser.CompanyId)
                    return Result<TaskType>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.ProjectNotOwnedTheCompany
                            }
                        }
                    );

                return Result<TaskType>.Success(taskType);
            }
            catch (Exception ex)
            {
                return Result<TaskType>.Error(ErrorHelper.GetExceptionError(ex));
            }

        }

        /// <summary>
        /// Validates that a task type fulfills all naming requirements
        /// </summary>
        /// <param name="taskType"></param>
        /// <returns></returns>
        public async Task<Result<TaskType>> ValidateNamingTaskTypeAsync(TaskType taskType)
        {
            try
            {
                if (!String.IsNullOrEmpty(taskType.Name))
                {
                    //Validate name only if name is not null or empty
                    var taskTypeNameDuplicated = await _taskTypeRepository.AnyAsync(
                                                            new TaskTypesByNameSpecification(
                                                                    name: taskType.Name,
                                                                    companyId: taskType.CompanyId,
                                                                    projectId: taskType.ProjectId
                                                                  )
                                                         );

                    if (taskTypeNameDuplicated)
                        return Result<TaskType>.Invalid(new List<ValidationError>(){
                    new()
                    {
                        ErrorMessage=ErrorStrings.TaskTypeNameDuplicated.Replace("[name]",taskType.Name)
                    }
                    });
                }


                if (!String.IsNullOrEmpty(taskType.Description))
                {
                    //Validate description only if name is not null or empty
                    var taskTypeDescriptionDuplicated = await _taskTypeRepository.AnyAsync(
                                                                    new TaskTypesByDescriptionSpecification(
                                                                            description: taskType.Description,
                                                                            companyId: taskType.CompanyId,
                                                                            projectId: taskType.ProjectId
                                                                         )
                                                                );

                    if (taskTypeDescriptionDuplicated)
                        return Result<TaskType>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.TaskTypeDescriptionDuplicated.Replace("[description]",taskType.Description)
                            }
                        }
                        );
                }


                return Result<TaskType>.Success(taskType);
            }
            catch (Exception ex)
            {
                return Result<TaskType>.Error(ErrorHelper.GetExceptionError(ex));
            }

        }

        public async Task<Result<TaskType>> ValidateTaskTypeBelongsToProjectAsync(int taskTypeId, int projectId)
        {
            try
            {
                //Get task type from the db
                var dbTaskType = await _taskTypeRepository.GetByIdAsync(taskTypeId);


                if (dbTaskType is null)
                    return Result<TaskType>.Invalid(new List<ValidationError>(){
                            new()
                            {
                                ErrorMessage=ErrorStrings.TaskTypeNotFound.Replace("[id]",taskTypeId.ToString())
                            }
                        });

                //Only allow to change task type under the same project
                if (dbTaskType.ProjectId != projectId)
                    return Result<TaskType>.Invalid(new List<ValidationError>(){
                            new()
                            {
                                ErrorMessage=ErrorStrings.TaskTypeNotOwnedByProject.Replace("[id]",taskTypeId.ToString())
                            }
                        });

                return Result<TaskType>.Success(dbTaskType);
            }
            catch (Exception ex)
            {
                return Result<TaskType>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        public async Task<Result<List<TaskType>>> PatchTaskTypesAsync(int[] taskTypesIds, JsonPatchDocument<TaskType> patchDocument, string userId)
        {
            // In case the array comes with duplicates, remove duplicates from taskTypesIds
            taskTypesIds = taskTypesIds.Distinct().ToArray();
            List<TaskType> resultList = new();

            if (taskTypesIds is null || taskTypesIds.Length == 0)
            {
                return Result<List<TaskType>>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.EmptyProvidedList
                    }
                });
            }

            List<int> missingTaskTypeIds = new List<int>();

            foreach (int taskTypeId in taskTypesIds)
            {
                TaskType? taskType = await _taskTypeRepository.GetByIdAsync(taskTypeId);

                if (taskType is null)
                {
                    missingTaskTypeIds.Add(taskTypeId);
                    continue;
                }

                Result<TaskType> accessResult = await ValidateAccessTaskTypeAsync(taskType, userId);

                if (!accessResult.IsSuccess)
                {
                    return Result<List<TaskType>>.Invalid(accessResult.ValidationErrors);
                }

                Result patchValidationResult = PatchValidationHelper.ValidatePatchDocument(patchDocument);

                if (!patchValidationResult.IsSuccess)
                {
                    return Result<List<TaskType>>.Error(patchValidationResult.Errors.ToArray());
                }

                patchDocument.ApplyTo(taskType);
                resultList.Add(taskType);
            }

            if (missingTaskTypeIds.Count > 0)
            {
                string errorMessage = ErrorStrings.TaskTypeNotFound.Replace("[id]", string.Join(", ", missingTaskTypeIds));
                return Result<List<TaskType>>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = errorMessage
                    }
                });
            }

            await _taskTypeRepository.SaveChangesAsync();

            return Result<List<TaskType>>.Success(resultList);
        }

        public async Task<Result<NoContentResult>> ActivateTaskTypesListAsync(int[] taskTypesIds, string userId)
        {
            // In case the array comes with duplicates, remove duplicates from taskTypesIds
            taskTypesIds = taskTypesIds.Distinct().ToArray();

            if (taskTypesIds is null || taskTypesIds.Length == 0)
            {
                return Result<NoContentResult>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.EmptyProvidedList
                    }
                });
            }

            var result = await SetTaskTypesActiveStatus(taskTypesIds, userId, true);

            if (!result.IsSuccess)
            {
                return Result<NoContentResult>.Invalid(result.ValidationErrors);
            }

            var (missingTaskTypeIds, resultList) = result.Value;

            if (missingTaskTypeIds.Count > 0)
            {
                string errorMessage = ErrorStrings.TaskTypeNotFound.Replace("[id]", string.Join(", ", missingTaskTypeIds));
                return Result<NoContentResult>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = errorMessage
                    }
                });
            }

            await _taskTypeRepository.SaveChangesAsync();

            return Result<NoContentResult>.Success(
                new NoContentResult()
            );
        }

        public async Task<Result<NoContentResult>> DeactivateTaskTypesListAsync(int[] taskTypesIds, string userId)
        {
            // In case the array comes with duplicates, remove duplicates from taskTypesIds
            taskTypesIds = taskTypesIds.Distinct().ToArray();

            if (taskTypesIds is null || taskTypesIds.Length == 0)
            {
                return Result<NoContentResult>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.EmptyProvidedList
                    }
                });
            }

            var result = await SetTaskTypesActiveStatus(taskTypesIds, userId, false);

            if (!result.IsSuccess)
            {
                return Result<NoContentResult>.Invalid(result.ValidationErrors);
            }

            var (missingTaskTypeIds, resultList) = result.Value;

            if (missingTaskTypeIds.Count > 0)
            {
                string errorMessage = ErrorStrings.TaskTypeNotFound.Replace("[id]", string.Join(", ", missingTaskTypeIds));
                return Result<NoContentResult>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = errorMessage
                    }
                });
            }

            await _taskTypeRepository.SaveChangesAsync();

            return Result<NoContentResult>.Success(
                new NoContentResult()
            );
        }

        public async Task<Result<PaginatedRecord<GetTaskTypesDto>>> GetTaskTypeRecentAsync(int companyUserId, int projectId, string search, int pageNumber, int perPage)
        {
            try
            {
                // Verify if companyUserId exist
                var companyUser = await _companyUserRepository.GetByIdAsync(companyUserId);

                if (companyUser is null)
                    return Result<PaginatedRecord<GetTaskTypesDto>>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.CompanyUserNotFound
                            }
                        }
                    );
                
                // Verify projectId exist
                var project = await _projectRepository.GetByIdAsync(projectId);

                if(project is null)
                    return Result<PaginatedRecord<GetTaskTypesDto>>.Invalid(new List<ValidationError>(){
                        new()
                            {
                                ErrorMessage=ErrorStrings.ProjectNotFound
                            }
                        }
                    );

                var tasks = await _taskRepository.ListAsync(new GetTaskTypesRecentlySpecification(companyUserId, projectId, search, pageNumber, perPage, true));
                var tasksDistinct = tasks.DistinctBy(task => task.Id);

                var paginatedRecords = new PaginatedRecord<GetTaskTypesDto>(tasksDistinct.ToList(), tasksDistinct.ToList().Count, perPage, pageNumber);

                return Result.Success(paginatedRecords);
            }
            catch (Exception ex)
            {
                return Result.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        #region Private methods

        private async Task<Result<(List<int> missingTaskTypeIds, List<TaskType> updatedTaskTypes)>> SetTaskTypesActiveStatus(
            int[] taskTypesIds,
            string userId,
            bool activeStatus
        )
        {
            List<int> missingTaskTypeIds = new();
            List<TaskType> resultList = new();

            foreach (int taskTypeId in taskTypesIds)
            {
                TaskType? taskType = await _taskTypeRepository.GetByIdAsync(taskTypeId);

                if (taskType is null)
                {
                    missingTaskTypeIds.Add(taskTypeId);
                    continue;
                }

                Result<TaskType> accessResult = await ValidateAccessTaskTypeAsync(taskType, userId);

                if (!accessResult.IsSuccess)
                {
                    return Result<(
                        List<int> missingTaskTypeIds,
                        List<TaskType> updatedTaskTypes
                    )>.Invalid(accessResult.ValidationErrors);
                }

                taskType.IsActive = activeStatus;
                resultList.Add(taskType);
            }

            return Result<(
                List<int> missingTaskTypeIds,
                List<TaskType> updatedTaskTypes
            )>.Success((missingTaskTypeIds, resultList));
        }

        #endregion
    }
}
