using Ardalis.Result;
using NetForemost.Core.Dtos.Timer;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Tasks
{
    public interface ITaskService
    {
        /// <summary>
		/// Get all the tasks
		/// </summary>
		/// <returns>Search for all tasks or provide the search parameters needed.</returns>
		Task<Result<PaginatedRecord<GetTasksQueryableDto>>> GetTasksAsync(
            string userId,
            string search,
            int typeId,
            int goalId,
            int projectId,
            int companyId,
            DateTime? targetEndDateFrom,
            DateTime? targetEndDateTo,
            int pageNumber = 1, int perPage = 10
        );

        /// <summary>
        /// Create a new task.
        /// </summary>
        /// <param name="task">The task to create.</param>
        /// <param name="userId">The user id who will create the task.</param>
        /// <returns>Task type created.</returns>
        Task<Result<Entities.Tasks.Task>> CreateTaskAsync(string userId, string description, DateTime targeEndDate, int goalId, int typeId);

        /// <summary>
        /// Update an existing task.
        /// </summary>
        /// <param name="taskType">The task to create.</param>
        /// <param name="userId">The user id who will create the task.</param>
        /// <returns>Task type created.</returns>
        public Task<Result<bool>> UpdateTaskAsync(Entities.Tasks.Task task, string userId);

        Task<Result<GetTasksQueryableDto>> GetTasksByIdAsync(int taskId, string userId);
    }
}
