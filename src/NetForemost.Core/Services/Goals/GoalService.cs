using Ardalis.Result;
using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.Core.Specifications.Goals;
using NetForemost.Core.Specifications.ProjectCompanyUser;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Goals;
public class GoalService : IGoalService
{
    private readonly IAsyncRepository<Goal> _goalRepository;
    private readonly IAsyncRepository<GoalExtraMile> _extraMileGoalRepository;
    private readonly IAsyncRepository<Project> _projectRepository;
    private readonly IAsyncRepository<GoalStatus> _goalStatusRepository;
    private readonly UserManager<User> _userManager;
    private readonly IAsyncRepository<PriorityLevel> _priorityLevelRepository;
    private readonly IAsyncRepository<StoryPoint> _storyPointRepository;
    private readonly IAsyncRepository<ProjectCompanyUser> _projectCompanyUserRepository;

    public GoalService(IAsyncRepository<Goal> goalRepository, IAsyncRepository<GoalExtraMile> extraMileGoalRepository, IAsyncRepository<Project> projectRepository, IAsyncRepository<GoalStatus> goalStatusRepository, UserManager<User> userManager, IAsyncRepository<PriorityLevel> priorityLevelRepository, IAsyncRepository<StoryPoint> storyPointRepository, IAsyncRepository<ProjectCompanyUser> projectCompanyUserRepository)
    {
        _goalRepository = goalRepository;
        _extraMileGoalRepository = extraMileGoalRepository;
        _projectRepository = projectRepository;
        _goalStatusRepository = goalStatusRepository;
        _userManager = userManager;
        _priorityLevelRepository = priorityLevelRepository;
        _storyPointRepository = storyPointRepository;
        _projectCompanyUserRepository = projectCompanyUserRepository;
    }

    public async Task<Result<Goal>> CreateGoal(Goal goal, string userId, double timeZone)
    {
        try
        {
            //Get project
            var project = await _projectRepository.GetByIdAsync(goal.ProjectId);

            // Get Verify if user belong to the project
            var projectCompanyUser = await _projectCompanyUserRepository.FirstOrDefaultAsync(new GetProjectByUserId(userId, project.Id, project.CompanyId));
            if (projectCompanyUser is null)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.ProjectNotFound.Replace("[id]", goal.ProjectId.ToString())
                        }
                    });

            //verify scrum master
            var scrumMaster = await _userManager.FindByIdAsync(goal.ScrumMasterId);
            if (scrumMaster is null)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.ScrumMasterNotFound
                        }
                    });

            //verify target date
            if (goal.TargetEndDate < goal.StartDate)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.InvalidGoal_EndDateLessThanStarDate
                        }
                    });

            // Verify goalStatusId
            var existGoalStatus = await _goalStatusRepository.GetByIdAsync((int)goal.GoalStatusId);
            if (existGoalStatus is null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.GoalStatus_NotFound
                        }
                    });
            }

            //Verify if goal have estimatedHours
            if (goal.EstimatedHours <= 0)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.EstimatedHourNotValid
                        }
                    });
            }

            //verify storypoint
            var existPriorityLevel = await _priorityLevelRepository.GetByIdAsync(goal.PriorityLevelId);
            if (existPriorityLevel is null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.PriorityLevelNotFound
                        }
                    });
            }

            //verify priority level

            var existStoryPoint = await _storyPointRepository.GetByIdAsync(goal.StoryPointId);
            if (existStoryPoint is null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.StoryPointNotFound
                        }
                    });
            }
            var ap = -timeZone;

            goal.StartDate = goal.StartDate.AddHours(-timeZone);
            goal.TargetEndDate = goal.TargetEndDate.AddHours(-timeZone);
            goal.CreatedAt = DateTime.UtcNow;
            goal.CreatedBy = userId;
            goal.OwnerId = projectCompanyUser.Id;

            var createdGoal = await _goalRepository.AddAsync(goal);

            createdGoal.StartDate = createdGoal.StartDate.AddHours(timeZone);
            createdGoal.TargetEndDate = createdGoal.TargetEndDate.AddHours(timeZone);
            createdGoal.Project = project;
            createdGoal.PriorityLevel = existPriorityLevel;
            createdGoal.StoryPoint = existStoryPoint;
            createdGoal.ScrumMaster = scrumMaster;
            createdGoal.GoalStatus = existGoalStatus;

            return Result<Goal>.Success(createdGoal);
        }
        catch (Exception ex)
        {
            return Result<Goal>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Goal>> ConfirmGoal(int goalId, int goalStatusId, string userId)
    {
        try
        {
            //verify goal exists and belongs to the user
            Goal? goal = await _goalRepository.GetByIdAsync(goalId);
            if (goal is null)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.GoalNotFound
                        }
                    });

            //verify it belongs to the user
            var checkUser = await _projectCompanyUserRepository.AnyAsync(new CheckUserExistInProjectSpecification(userId, goal.Owner.ProjectId));
            if (!checkUser)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.Goal_NotOwnedByUser
                        }
                    });

            // Verify goalStatusId
            goal.GoalStatus = await _goalStatusRepository.GetByIdAsync(goalStatusId);
            if (goal.GoalStatus is null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.GoalStatus_NotFound
                        }
                    });
            }

            //Update actual end date
            goal.UpdatedAt = DateTime.UtcNow;
            goal.ActualEndDate = DateTime.UtcNow;
            goal.GoalStatusId = goalStatusId;

            await _goalRepository.UpdateAsync(goal);

            return Result<Goal>.Success(goal);
        }
        catch (Exception ex)
        {
            return Result<Goal>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<GoalExtraMile>> ConfirmExtraMileGoal(int goalId, int goalStatusId, string userId)
    {
        try
        {
            try
            {
                ActiveExtraMileGoalByIdSpecification specification = new ActiveExtraMileGoalByIdSpecification(goalId, userId, includeRelations: true);
                GoalExtraMile? extraMileGoal = await _extraMileGoalRepository.GetBySpecAsync(specification);

                if (extraMileGoal is null)
                    return Result<GoalExtraMile>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.GoalNotFound
                        }
                    });

                // Verify goalStatusId
                var existGoalStatus = await _goalStatusRepository.GetByIdAsync(goalStatusId);

                if (existGoalStatus is null)
                {
                    return Result<GoalExtraMile>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.GoalStatus_NotFound
                        }
                    });
                }

                //Update actual end date
                extraMileGoal.UpdatedAt = DateTime.UtcNow;
                extraMileGoal.ActualEndDate = DateTime.UtcNow;
                extraMileGoal.GoalStatusId = goalStatusId;

                await _extraMileGoalRepository.UpdateAsync(extraMileGoal);

                //Add navegation entities
                extraMileGoal.GoalStatus = existGoalStatus;

                return Result<GoalExtraMile>.Success(extraMileGoal);
            }
            catch (Exception ex)
            {
                return Result<GoalExtraMile>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }
        catch (Exception ex)
        {
            return Result<GoalExtraMile>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<GoalExtraMile>> CreateExtraMileGoal(GoalExtraMile extraMileGoal, string userId, double timeZone)
    {
        try
        {
            extraMileGoal.ExtraMileTargetEndDate = extraMileGoal.ExtraMileTargetEndDate.AddHours(-timeZone);

            //verify goal exists
            var existGoal = await _goalRepository.GetByIdAsync(extraMileGoal.GoalId);
            if (existGoal is null)
                return Result<GoalExtraMile>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.GoalNotFound.Replace("[id]", extraMileGoal.GoalId.ToString())
                        }
                    });

            //Verify the goal hasn't been marked as completed already 
            if (existGoal.ActualEndDate is not null)
                return Result<GoalExtraMile>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.Goal_CompletedAlready_Error
                        }
                    });

            // Verify goalStatusId
            var existGoalStatus = await _goalStatusRepository.GetByIdAsync(extraMileGoal.GoalStatusId);
            if (existGoalStatus is null)
            {
                return Result<GoalExtraMile>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.GoalStatus_NotFound
                        }
                    });
            }

            //Mark the goal to know it has an extra mile goal
            if (!existGoal.HasExtraMileGoal)
            {
                Goal goal = existGoal;
                goal.HasExtraMileGoal = true;
                goal.GoalStatusId = null;
                await _goalRepository.UpdateAsync(goal);
            }

            //Verify previous extra mile goals, associated with this goal
            var extraMileGoalsByGoalIdSpecification = new ExtraMileGoalsByGoalIdSpecification(extraMileGoal.GoalId);
            List<GoalExtraMile> previousExtraMileGoals = await _extraMileGoalRepository.ListAsync(extraMileGoalsByGoalIdSpecification);
            if (previousExtraMileGoals.Count > 0)
            {
                //We need to void the last extra mile goal
                var lastExtraMile = previousExtraMileGoals.Last();
                if (lastExtraMile.IsVoided is not true)
                {
                    lastExtraMile.IsVoided = true;
                    lastExtraMile.UpdatedBy = userId;
                    lastExtraMile.UpdatedAt = DateTime.Now;
                    await _extraMileGoalRepository.UpdateAsync(lastExtraMile);
                }
            }

            //verify target date is less than initial goal date
            if (extraMileGoal.ExtraMileTargetEndDate > existGoal.TargetEndDate)
                return Result<GoalExtraMile>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.InvalidGoal_EndDateLessThanStarDate
                        }
                    });

            //verify if user belong to the project 
            var goalRelationData = await _goalRepository.GetBySpecAsync(new GetGoalWithRelationDataSpecification(extraMileGoal.GoalId));

            var user = await _userManager.FindByIdAsync(goalRelationData.Owner.CompanyUser.UserId);

            var userBelongToProject = await _projectCompanyUserRepository.AnyAsync(new CheckUserExistInProjectSpecification(user.Id, existGoal.ProjectId));

            if (!userBelongToProject)
            {
                return Result<GoalExtraMile>.Invalid(new List<ValidationError>()
                {
                    new()
                    {
                        ErrorMessage=ErrorStrings.UserNotBelongToProject
                    }
                });
            }

            extraMileGoal.CreatedAt = DateTime.UtcNow;
            extraMileGoal.CreatedBy = userId;
            extraMileGoal.IsActive = true;

            var createdGoal = await _extraMileGoalRepository.AddAsync(extraMileGoal);
            //add navegation entities


            return Result<GoalExtraMile>.Success(createdGoal);
        }
        catch (Exception ex)
        {
            return Result<GoalExtraMile>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<FindAllGoalsDto>>> FindAllGoalsAsync(
        string userId,
        string description,
        double estimatedHours,
        int projectId,
        int storyPoints,
        DateTime? dateStartTo, DateTime? dateStartFrom,
        DateTime? actualendDateTo, DateTime? actualendDateFrom,
        DateTime CreationDateTo, DateTime CreationDateFrom,
        string scrumMasterId,
        string jiraTicketId,
        string priorityLevel,
        double timeZone,
        int? goalStatusId,
        int companyId,
        int pageNumber, int perPage
        )
    {
        try
        {
            //count the records
            var count = await _goalRepository.CountAsync(
                new FindAllGoalsSpecification(
                    userId,
                    description,
                    estimatedHours,
                    projectId,
                    storyPoints,
                    dateStartTo, dateStartFrom,
                    actualendDateTo, actualendDateFrom,
                    scrumMasterId,
                    jiraTicketId,
                    priorityLevel,
                    goalStatusId,
                    timeZone,
                    pageNumber, perPage,
                    CreationDateTo, CreationDateFrom,
                    companyId
                )
            );

            //the records of project
            var goals = await _goalRepository.ListAsync(
                new FindAllGoalsSpecification(
                    userId,
                    description,
                    estimatedHours,
                    projectId,
                    storyPoints,
                    dateStartTo, dateStartFrom,
                    actualendDateTo, actualendDateFrom,
                    scrumMasterId,
                    jiraTicketId,
                    priorityLevel,
                    goalStatusId,
                    timeZone,
                    pageNumber, perPage,
                    CreationDateTo, CreationDateFrom, companyId, true
                )
            );

            // Paginate result
            var paginatedRecords = new PaginatedRecord<FindAllGoalsDto>(goals, count, perPage, pageNumber);

            return Result.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<GoalExtraMile>>> FindAllGoalsExtraMileAsync(
        string userId,
        int goalId,
        string description,
        int projectId,
        int storyPoints,
        DateTime? dateStartTo, DateTime? dateStartFrom,
        DateTime? actualendDateTo, DateTime? actualendDateFrom,
        string scrumMasterId,
        string jiraTicketId,
        string priorityLevel,
        int goalStatusId,
        double timeZone,
        int pageNumber, int perPage)
    {
        try
        {
            //count the records
            var count = await _extraMileGoalRepository.CountAsync(
                new FindAllGoalsExtraMileSpecification(
                    userId,
                    goalId,
                    description,
                    projectId,
                    storyPoints,
                    dateStartTo, dateStartFrom,
                    actualendDateTo, actualendDateFrom,
                    scrumMasterId,
                    jiraTicketId,
                    priorityLevel,
                    goalStatusId,
                    timeZone,
                    pageNumber, perPage
                )
            );

            //the records of project
            var goals = await _extraMileGoalRepository.ListAsync(
                new FindAllGoalsExtraMileSpecification(
                    userId,
                    goalId,
                    description,
                    projectId,
                    storyPoints,
                    dateStartTo, dateStartFrom,
                    actualendDateTo, actualendDateFrom,
                    scrumMasterId,
                    jiraTicketId,
                    priorityLevel,
                    goalStatusId,
                    timeZone,
                    pageNumber, perPage, true
                )
            );

            // Paginate result
            var paginatedRecords = new PaginatedRecord<GoalExtraMile>(goals, count, perPage, pageNumber);

            return Result<PaginatedRecord<GoalExtraMile>>.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result<PaginatedRecord<GoalExtraMile>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<ActiveGoalsSummaryDto>> GetActiveGoalsSummary(string userId, double timeZone)
    {
        try
        {
            var activeGoalsSummaryDto = new ActiveGoalsSummaryDto();
            //Get all data we will need
            var activeGoals = await GetActiveGoals(userId, timeZone);
            var activeExtraMileGoal = await GetActiveExtraMileGoals(userId, timeZone);

            //Extra Mile Goals
            activeGoalsSummaryDto.ExtraMileGoals = activeExtraMileGoal.Value.Count;

            //Standard Goal
            activeGoalsSummaryDto.ActiveGoals = activeGoals.Value.Count;


            //On Time Goal
            int activeOnTimeGoals = activeGoals.Value.Where(goal => goal.TargetEndDate > DateTime.Now).ToList().Count;
            int activeOnTimeExtraMileGoals = activeExtraMileGoal.Value.Where(extraMileGoal => extraMileGoal.ExtraMileTargetEndDate > DateTime.Now).ToList().Count;

            activeGoalsSummaryDto.OnTimeGoals = activeOnTimeGoals + activeOnTimeExtraMileGoals;

            //Late Goals
            int activeLateGoals = activeGoals.Value.Where(goal => goal.TargetEndDate <= DateTime.Now).ToList().Count;
            int activeLateExtraMileGoals = activeExtraMileGoal.Value.Where(extraMileGoal => extraMileGoal.ExtraMileTargetEndDate <= DateTime.Now).ToList().Count;

            activeGoalsSummaryDto.LateGoals = activeLateGoals + activeLateExtraMileGoals;


            return Result<ActiveGoalsSummaryDto>.Success(activeGoalsSummaryDto);

        }
        catch (Exception ex)
        {
            return Result<ActiveGoalsSummaryDto>.Error(ErrorHelper.GetExceptionError(ex));
        }

    }
    public async Task<Result<List<Goal>>> GetActiveGoals(string userId, double timeZone)
    {
        try
        {
            var goals = await _goalRepository.ListAsync(
                new ActiveGoalsSpecification(userId, timeZone, includeRelations: true)
            );

            return Result<List<Goal>>.Success(goals);
        }
        catch (Exception ex)
        {
            return Result<List<Goal>>.Error(ErrorHelper.GetExceptionError(ex));
        }

    }

    public async Task<Result<List<GoalExtraMile>>> GetActiveExtraMileGoals(string userId, double timeZone)
    {
        try
        {

            var extraMileGoalsSpec = new ActiveExtraMileGoalsSpecification(userId, timeZone);
            List<GoalExtraMile> extraMileGoals = await _extraMileGoalRepository.ListAsync(extraMileGoalsSpec);


            return Result<List<GoalExtraMile>>.Success(extraMileGoals);
        }
        catch (Exception ex)
        {
            return Result<List<GoalExtraMile>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Goal>> ValidateActiveGoalBelongsToUser(int goalId, string userId)
    {
        try
        {
            //verify goal exists and belongs to the user
            Goal? goal = await _goalRepository.GetByIdAsync(goalId);

            if (goal is null)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.GoalNotFound
                        }
                    });

            //verify it belongs to the user
            var checkUser = await _projectCompanyUserRepository.AnyAsync(new CheckUserExistInProjectSpecification(userId, goal.ProjectId));
            if (!checkUser)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.Goal_NotOwnedByUser
                        }
                    });

            if (goal.ActualEndDate != null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.GoalNotActive
                        }
                    });
            }

            return Result<Goal>.Success(goal);
        }
        catch (Exception ex)
        {
            return Result<Goal>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public Result<Goal> ValidateActiveGoalBelongsToProject(Goal goal, int projectId)
    {
        try
        {

            //verify it belongs to the project
            if (goal.ProjectId != projectId)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.GoalNotOwnedByProject
                        }
                    });


            return Result<Goal>.Success(goal);
        }
        catch (Exception ex)
        {
            return Result<Goal>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Goal>> UpdateStatusGoal(int goalId, int goalStatusId, string userId)
    {
        try
        {
            //verify goal exists
            var goal = await _goalRepository.FirstOrDefaultAsync(new GetGoalByIdAndUserId(goalId, userId));
            if (goal is null)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.GoalNotFound
                        }
                    });

            //verify it belongs to the user
            var checkUser = await _projectCompanyUserRepository.AnyAsync(new CheckUserExistInProjectSpecification(userId, goal.Owner.ProjectId));
            if (!checkUser)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.Goal_NotOwnedByUser
                        }
                    });

            // Verify the new goalStatusId
            var goalStatus = await _goalStatusRepository.GetByIdAsync(goalStatusId);

            if (goalStatus is null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.GoalStatus_NotFound
                        }
                    });
            }

            goal.UpdateStausGoal(goalStatus, userId);

            // Verify is goal is extramile
            var goalExtraMile = await _extraMileGoalRepository.GetBySpecAsync(new FindExtraMileByGoalIdSpecification(goalId));

            if (goal.HasExtraMileGoal && goalExtraMile is not null && !goalStatus.IsFinalStatus)
            {
                // The goal is no longer extra mile
                goal.HasExtraMileGoal = false;

                goalExtraMile.CancellExtraMile(userId);

                await _extraMileGoalRepository.SaveChangesAsync();
            }

            await _goalRepository.SaveChangesAsync();
            goal.GoalStatus = goalStatus;

            return Result<Goal>.Success(goal);
        }
        catch (Exception ex)
        {
            return Result<Goal>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Goal>> UpdateGoal(Goal goalNewData, string userId)
    {
        try
        {
            // Verify if goal exist
            var goal = await _goalRepository.GetByIdAsync(goalNewData.Id);

            if (goal is null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.GoalNotFound
                        }
                    });
            }

            //Get project
            var project = await _projectRepository.GetByIdAsync(goal.ProjectId);

            // Get Verify if user belong to the project
            var projectCompanyUser = await _projectCompanyUserRepository.FirstOrDefaultAsync(new GetProjectByUserId(userId, goalNewData.ProjectId, project.CompanyId));
            if (projectCompanyUser is null)
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage=ErrorStrings.ProjectNotFound.Replace("[id]", goalNewData.ProjectId.ToString())
                        }
                    });

            // Verify if new Scrum master exist
            var scrumMaster = await _userManager.FindByIdAsync(goalNewData.ScrumMasterId);

            if (scrumMaster is null)
            {
                return Result<Goal>.Invalid(new List<ValidationError>()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.ScrumMasterNotFound
                        }
                    });
            }

            goalNewData.OwnerId = projectCompanyUser.Id;
            goal.UpdateGoal(goalNewData, userId);

            await _goalRepository.SaveChangesAsync();

            return Result<Goal>.Success(goalNewData);
        }
        catch (Exception ex)
        {
            return Result<Goal>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<GetLateGoalDto>>> GetLateGoals(string userId, DateTime from, DateTime to, int perPage, int pageNumber, double timeZone)
    {
        try
        {
            var goals = await _goalRepository.ListAsync(new GetGoalCompletedLateByUserIdAndDateRange(userId, from, to, pageNumber, perPage, true, timeZone));
            var goalsCount = await _goalRepository.CountAsync(new GetGoalCompletedLateByUserIdAndDateRange(userId, from, to, pageNumber, perPage, false, timeZone));

            var paginatedRecord = new PaginatedRecord<GetLateGoalDto>(goals, goalsCount, perPage, pageNumber);

            return paginatedRecord;
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<GetCompletedGoalDto>>> GetCompletedGoals(string userId, DateTime from, DateTime to, int perPage, int pageNumber, double timeZone)
    {
        try
        {
            var goals = await _goalRepository.ListAsync(new GetGoalCompletedByUserIdAndDateRange(userId, from, to, pageNumber, perPage, true, timeZone));
            var goalsCount = await _goalRepository.CountAsync(new GetGoalCompletedByUserIdAndDateRange(userId, from, to, pageNumber, perPage, false, timeZone));

            var paginatedRecord = new PaginatedRecord<GetCompletedGoalDto>(goals, goalsCount, perPage, pageNumber);

            return paginatedRecord;
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}