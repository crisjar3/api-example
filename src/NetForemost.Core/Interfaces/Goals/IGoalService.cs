using Ardalis.Result;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Goals;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Goals
{
    public interface IGoalService
    {
        ///<summary>
        /// Get all the goals
        /// </summary>
        /// <returns>Search for all Goals or provide the search parameters needed.</returns>
        Task<Result<PaginatedRecord<FindAllGoalsDto>>> FindAllGoalsAsync(
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
        );

        /// <summary>
        /// Get all the goals
        /// </summary>
        /// <returns>Search for all Goals or provide the search parameters needed.</returns>
        Task<Result<PaginatedRecord<GoalExtraMile>>> FindAllGoalsExtraMileAsync(
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
            double timezone,
            int pageNumber = 1,
            int perPage = 10
        );

        /// <summary>
        /// Get all active goals only
        /// </summary>
        /// <returns>Listo of the current active goals of a user</returns>
        Task<Result<List<Goal>>> GetActiveGoals(string userId, double timeZone);

        /// <summary>
        /// Get summary of active goals and extra mile goals
        /// </summary>
        /// <returns>Important summary of the current active goals of a user, in order to use as KPIs</returns>
        Task<Result<ActiveGoalsSummaryDto>> GetActiveGoalsSummary(string userId, double timeZone);

        /// <summary>
        /// Get active extra mile goals
        /// </summary>
        /// <returns>A list of extra mile goals</returns>
        Task<Result<List<GoalExtraMile>>> GetActiveExtraMileGoals(string userId, double timeZone);

        ///<summary>
        /// Create a new goal.
        /// </summary>
        /// <returns>A new goal created.</returns>
        Task<Result<Goal>> CreateGoal(Goal goal, string userId, double timeZone);

        ///<summary>
        /// Confirm a goal has been completed.
        /// </summary>
        /// <returns>The confirmed goal.</returns>
        Task<Result<Goal>> ConfirmGoal(int goalId, int goalStatusId, string userId);

        ///<summary>
        /// Confirm an extra mile goal has been completed.
        /// </summary>
        /// <returns>The confirmed extra mile goal.</returns>
        Task<Result<GoalExtraMile>> ConfirmExtraMileGoal(int extraMileGoalId, int goalStatusId, string userId);

        ///<summary>
        /// Create a new extra mile goal.
        /// </summary>
        /// <returns>A new extra mile goal created.</returns>
        Task<Result<GoalExtraMile>> CreateExtraMileGoal(GoalExtraMile goal, string userId, double timeZone);

        ///<summary>
        /// Validate a goal is active and it belongs to a user.
        /// </summary>
        /// <returns>The goal.</returns>
        Task<Result<Goal>> ValidateActiveGoalBelongsToUser(int goalId, string userId);

        ///<summary>
        /// Validate a goal belongs to the provided Project
        /// </summary>
        /// <returns>The goal.</returns>
        Result<Goal> ValidateActiveGoalBelongsToProject(Goal goal, int projectId);

        ///<summary>
        /// Revert a goal to in progress if this has been completed.
        /// </summary>
        /// <returns>The goal in progress.</returns>
        Task<Result<Goal>> UpdateStatusGoal(int goalId, int goalStatusId, string userId);

        ///<summary>
        /// Update specific information of a goal.
        /// </summary>
        /// <returns>The goal update.</returns>
        Task<Result<Goal>> UpdateGoal(Goal goalNewData, string userId);

        /// <summary>
        /// Get tha goals completed but late in range of time paginated
        /// </summary>
        /// <param name="userId">the woner of the goals</param>
        /// <param name="from">date range from</param>
        /// <param name="to">date range to</param>
        /// <param name="perPage">quantity of register per page</param>
        /// <param name="pageNumber">number of page specified</param>
        /// <returns>the records of the goals completed on late</returns>
        Task<Result<PaginatedRecord<GetLateGoalDto>>> GetLateGoals(string userId, DateTime from, DateTime to, int perPage, int pageNumber, double timeZone);

        /// <summary>
        /// Get the recors of the goals completed correctly
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="perPage"></param>
        /// <param name="pageNumber"></param>
        /// <returns>the records of the goals completed on time</returns>
        Task<Result<PaginatedRecord<GetCompletedGoalDto>>> GetCompletedGoals(string userId, DateTime from, DateTime to, int perPage, int pageNumber, double timeZone);
    }
}
