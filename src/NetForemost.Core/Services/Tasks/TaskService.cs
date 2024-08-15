using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.Core.Specifications.Goals;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IAsyncRepository<Entities.Tasks.Task> _taskRepository;
        private readonly IAsyncRepository<TaskType> _taskTypeRepository;

        private readonly ITaskTypeService _taskTypeService;
        private readonly IGoalService _goalService;
        private readonly UserManager<User> _userManager;
        private readonly IAsyncRepository<Goal> _goalRepository;

        public TaskService(IAsyncRepository<Entities.Tasks.Task> taskRepository,
                           IAsyncRepository<TaskType> taskTypeRepository,
                           ITaskTypeService taskTypeService,
                           IGoalService goalService,
                           UserManager<User> userManager,
                           IAsyncRepository<Goal> goalRepository)
        {
            _taskRepository = taskRepository;
            _taskTypeRepository = taskTypeRepository;
            _taskTypeService = taskTypeService;
            _goalService = goalService;
            _userManager = userManager;
            _goalRepository = goalRepository;
        }

        public async Task<Result<PaginatedRecord<GetTasksQueryableDto>>> GetTasksAsync(
            string userId,
            string search,
            int typeId,
            int goalId,
            int projectId,
            int companyId,
            DateTime? targetEndDateFrom,
            DateTime? targetEndDateTo,
            int pageNumber, int perPage)
        {
            try
            {

                //count the records
                var count = await _taskRepository.CountAsync(
                    new TasksQueryableSpecification(
                        userId: userId,
                        search: search,
                        typeId: typeId,
                        goalId: goalId,
                        projectId: projectId,
                        companyId: companyId,
                        targetEndDateFrom: targetEndDateFrom,
                        targetEndDateTo: targetEndDateTo,
                        pageNumber: pageNumber,
                        perPage: perPage
                    )
                );

                //the records of project
                var tasks = await _taskRepository.ListAsync(
                    new TasksQueryableSpecification(
                        userId: userId,
                        search: search,
                        typeId: typeId,
                        goalId: goalId,
                        projectId: projectId,
                        companyId: companyId,
                        targetEndDateFrom: targetEndDateFrom,
                        targetEndDateTo: targetEndDateTo,
                        pageNumber: pageNumber,
                        perPage: perPage
                    )
                );

                // Paginate result
                var paginatedRecords = new PaginatedRecord<GetTasksQueryableDto>(tasks, count, perPage, pageNumber);

                return Result<PaginatedRecord<GetTasksQueryableDto>>.Success(paginatedRecords);
            }
            catch (Exception ex)
            {
                return Result.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        public async Task<Result<Entities.Tasks.Task>> CreateTaskAsync(string userId, string description, DateTime targeEndDate, int goalId, int typeId)
        {
            try
            {
                //Validate task type
                var dbTaskType = await _taskTypeRepository.GetByIdAsync(typeId);
                if (dbTaskType is null)
                    return Result<Entities.Tasks.Task>.Invalid(new List<ValidationError>(){
                            new()
                            {
                                ErrorMessage=ErrorStrings.TaskTypeNotFound.Replace("[id]",typeId.ToString())
                            }
                        });

                //Validate the goal, if provided
                var goal = new Goal();
                var task = new Entities.Tasks.Task();

                if (goalId > 0)
                {
                    //Validate belongs to user
                    var validateActiveGoalBelongsToUser = await _goalService.ValidateActiveGoalBelongsToUser(goalId, userId);
                    if (validateActiveGoalBelongsToUser.IsSuccess)
                    {
                        Goal activeGoal = validateActiveGoalBelongsToUser.Value;
                        task.ProjectId = activeGoal.ProjectId;
                        task.OwnerId = activeGoal.OwnerId;
                        goal = activeGoal;
                        task.Description = description;
                        task.TargetEndDate = targeEndDate;
                        task.GoalId = activeGoal.Id;

                    }
                    else
                        return Result<Entities.Tasks.Task>.Invalid(validateActiveGoalBelongsToUser.ValidationErrors);
                }

                var goalRelationData = await _goalRepository.FirstOrDefaultAsync(new GetGoalWithRelationDataSpecification(goal.Id));

                var user = await _userManager.FindByIdAsync(goalRelationData.Owner.CompanyUser.UserId);

                //Validate the task type to know if the user has access
                var validateAccessTaskType = await _taskTypeService.ValidateAccessTaskTypeAsync(dbTaskType, user.Id);
                if (!validateAccessTaskType.IsSuccess)
                {
                    return Result<Entities.Tasks.Task>.Invalid(validateAccessTaskType.ValidationErrors);
                }

                //Inherit values from the new dbTaskType
                task.TypeId = dbTaskType.Id;
                task.CompanyId = dbTaskType.CompanyId;

                //Set default values
                task.CreatedAt = DateTime.UtcNow;
                task.CreatedBy = userId;
                var taskCreated = await _taskRepository.AddAsync(task);

                return Result<Entities.Tasks.Task>.Success(taskCreated);


            }
            catch (Exception ex)
            {
                return Result<Entities.Tasks.Task>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        public async Task<Result<bool>> UpdateTaskAsync(Entities.Tasks.Task task, string userId)
        {
            try
            {
                //Get task from the db
                var dbTask = await _taskRepository.GetByIdAsync(task.Id);
                if (dbTask is null)
                    return Result<bool>.Invalid(new List<ValidationError>(){
                        new()
                        {
                            ErrorMessage=ErrorStrings.TaskNotFound.Replace("[id]",task.Id.ToString())
                        }
                });

                //Validate the taskType only if it has been changed
                if (dbTask.TypeId != task.TypeId)
                {
                    ////Validate the type, if provided 
                    if (task.TypeId > 0)
                    {
                        //Validate task type belongs to project and exists
                        var validateTaskTypeBelongsToProject = await _taskTypeService.ValidateTaskTypeBelongsToProjectAsync(task.TypeId, dbTask.ProjectId);

                        if (validateTaskTypeBelongsToProject.IsSuccess)
                        {
                            var dbTaskType = validateTaskTypeBelongsToProject.Value;

                            var goalRelationData = await _goalRepository.FirstOrDefaultAsync(new GetGoalWithRelationDataSpecification(dbTask.GoalId));

                            var user = await _userManager.FindByIdAsync(goalRelationData.Owner.CompanyUser.UserId);

                            //Validate the task type to know if the user has access
                            var validateAccessTaskType = await _taskTypeService.ValidateAccessTaskTypeAsync(dbTaskType, user.Id);
                            if (!validateAccessTaskType.IsSuccess)
                            {
                                return Result<bool>.Invalid(validateAccessTaskType.ValidationErrors);
                            }

                            //Inherit values from the new dbTaskType
                            dbTask.TypeId = dbTaskType.Id;
                            dbTask.CompanyId = dbTaskType.CompanyId;
                            dbTask.ProjectId = dbTaskType.ProjectId;
                        }
                        else
                        {
                            return Result<bool>.Invalid(validateTaskTypeBelongsToProject.ValidationErrors);
                        }
                    }
                }

                //Validate the Goal only if it has been changed
                if (dbTask.GoalId != task.GoalId)
                {
                    //Validate the goal, if provided
                    if (task.GoalId > 0)
                    {

                        //Validate belongs to user
                        var validateActiveGoalBelongsToUser = await _goalService.ValidateActiveGoalBelongsToUser((int)task.GoalId, userId);
                        if (!validateActiveGoalBelongsToUser.IsSuccess)
                        {
                            return Result<bool>.Invalid(validateActiveGoalBelongsToUser.ValidationErrors);

                        }
                        Goal activeGoal = validateActiveGoalBelongsToUser.Value;

                        // The task project id, must be the same project that the goal belongs too
                        var validateActiveGoalBelongsToProject = _goalService.ValidateActiveGoalBelongsToProject(activeGoal, dbTask.ProjectId);
                        if (!validateActiveGoalBelongsToProject.IsSuccess)
                        {
                            return Result<bool>.Invalid(validateActiveGoalBelongsToProject.ValidationErrors);
                        }

                        //Update dbTask
                        dbTask.GoalId = task.GoalId;
                    }

                }


                if (!String.IsNullOrEmpty(task.Description))
                    dbTask.Description = task.Description;
                if (task.TargetEndDate != DateTime.MinValue)
                    dbTask.TargetEndDate = task.TargetEndDate;

                //Set default values
                dbTask.UpdatedAt = DateTime.UtcNow;
                dbTask.UpdatedBy = userId;

                await _taskRepository.UpdateAsync(dbTask);

                return Result<bool>.Success(true);

            }
            catch (Exception ex)
            {
                return Result<bool>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

        public async Task<Result<GetTasksQueryableDto>> GetTasksByIdAsync(int taskId, string userId)
        {
            try
            {
                var task = await _taskRepository.FirstOrDefaultAsync(new GetTaskById(taskId, userId));

                if (task == null)
                {
                    return Result.Invalid(new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.TaskNotFound
                        }
                    });
                }

                return Result.Success(task);
            }
            catch (Exception ex)
            {
                return Result.Error(ErrorHelper.GetExceptionError(ex));
            }
        }

    }
}