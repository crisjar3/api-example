using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Tasks;
using NetForemost.Core.Entities.Tasks;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Tasks
{
    public interface ITaskTypeService
    {
        /// <summary>
		/// Get all the task types
		/// </summary>
		/// <returns>Search for all task types or provide the search parameters needed.</returns>
		Task<Result<PaginatedRecord<GetTaskTypesDto>>> GetTaskTypesAsync(
            string userId,
            string name,
            string description,
            int projectId,
            int companyId,
            int pageNumber = 1, int perPage = 10
        );

        /// <summary>
        /// Create a new task type.
        /// </summary>
        /// <param name="taskType">The task type to create.</param>
        /// <param name="userId">The user id who will create the project.</param>
        /// <returns>Task type created.</returns>
        public Task<Result<TaskType>> CreateTaskTypeAsync(TaskType taskType, string userId);

        /// <summary>
        /// Update an existing task type.
        /// </summary>
        /// <param name="taskType">The task type to create.</param>
        /// <param name="userId">The user id who will create the project.</param>
        /// <returns>Task type created.</returns>
        public Task<Result<TaskType>> UpdateTaskTypeAsync(TaskType taskType, string userId);

        /// <summary>
        /// Validates that a user has all the access requirements to use a task type
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Result<TaskType>> ValidateAccessTaskTypeAsync(TaskType taskType, string userId);

        /// <summary>
        /// Validates that a task type belongs to the project
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Result<TaskType>> ValidateTaskTypeBelongsToProjectAsync(int taskTypeId, int projectId);

        /// <summary>
        /// Updates the IsActive field for one or more task types
        /// </summary>
        /// <param name="taskTypesIds">The list of task types that are going to be updated</param>
        /// <param name="userId">The user identificator from the user making the request</param>
        /// <returns>Returns de list of task types with the updated fields</returns>
        public Task<Result<NoContentResult>> ActivateTaskTypesListAsync(int[] taskTypesIds, string userId);

        /// <summary>
        /// Updates the IsActive field for one or more task types
        /// </summary>
        /// <param name="taskTypesIds">The list of task types that are going to be updated</param>
        /// <param name="userId">The user identificator from the user making the request</param>
        /// <returns>Returns de list of task types with the updated fields</returns>
        public Task<Result<NoContentResult>> DeactivateTaskTypesListAsync(int[] taskTypesIds, string userId);

        /// <summary>
        /// Get all Task Types Recently created by a specific user
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="search"></param>
        /// <param name="pageNumber"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        Task<Result<PaginatedRecord<GetTaskTypesDto>>> GetTaskTypeRecentAsync(int companyUserId, int projectId, string search, int pageNumber, int perPage);

    }
}
