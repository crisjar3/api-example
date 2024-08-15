using Ardalis.Result;
using NetForemost.Core.Entities.JobRoles;

namespace NetForemost.Core.Interfaces.JobRoles;

public interface IJobRoleService
{
    /// <summary>
    /// Get all the job categories with all the job roles, no parameter is requested since it does not perform any search.
    /// </summary>
    /// <returns>A role category list with its roles</returns>
    Task<Result<List<JobRoleCategory>>> FindJobRoleCategoriesAsync(int CompanyId);
    /// <summary>
    ///create a new custom job role category.
    /// </summary>
    /// <returns>The job role category created.</returns>
    Task<Result<JobRoleCategory>> CreateCustomJobRoleCategoryAsync(JobRoleCategory jobRoleCategory, string userId);

    ///<summary>
    /// Create a new custom job role.
    /// </summary>
    /// <returns>A new role created.</returns>
    Task<Result<JobRole>> CreateCustomJobRoleAsync(JobRole jobRole, string userId);

    ///<summary>
    /// Get all job role of a company.
    /// <param name="companyId">The id of the company who will be looked for the roles.</param>
    /// <param name="userId">The user who is doing the request</param>
    /// </summary>
    /// <returns>A list of role of a company.</returns>
    Task<Result<List<JobRole>>> GetJobRolesAsync(int companyId, string userId);
}