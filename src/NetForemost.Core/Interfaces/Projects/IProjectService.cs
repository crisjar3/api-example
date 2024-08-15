using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Projects;
public interface IProjectService
{
    /// <summary>
    /// Create a new project.
    /// </summary>
    /// <param name="project">The project to create.</param>
    /// <param name="userId">The user id who will create the project.</param>
    /// <returns>Project created.</returns>
    Task<Result<Project>> CreateProjectAsync(Project project, string userId);

    /// <summary>
    /// edit a existing project.
    /// </summary>
    /// <param name="project">The project to update.</param>
    /// <param name="userId">The user who is to update the project.</param>
    /// <returns>Project edited.</returns>
    Task<Result<Project>> UpdateProjectAsync(Project project, string userId);

    /// <summary>
    /// Obtains the projects by several fields.
    /// </summary>
    /// <param name="name">The name of projects.</param>
    /// <param name="description">The name of projects.</param>
    /// <param name="companyId">The id of the company owning the project.</param>
    /// <param name="techStack">The list of technologies used in the project.</param>
    /// <param name="budgetRangeFrom">The range budget of the project.</param>
    /// <param name="budgetRangeTo">The range budget of the project.</param>
    /// <param name="dateEndFrom">The estimated completion date range.</param>
    /// <param name="dateEndTo">The estimated completion date range.</param>
    /// <param name="dateStartTo">The project start date range.</param>
    /// <param name="dateStartFrom">The project start date range.</param>
    /// <param name="pageNumber">Page number to display</param>
    /// <param name="perPage">Number of records per page</param>
    /// <param name="userIdFilter">The user assigned to the searched projects</param>
    /// <param name="userId">The user who is doing the request</param>
    /// <returns>All records resulting from projects.</returns>
    Task<Result<PaginatedRecord<ProjectStatusDto>>> FindProjectsAsync(
        int projectId,
        string name, string description,
        int companyId, string[] techStack,
        float budgetRangeFrom, float budgetRangeTo, string userId,
        DateTime? dateStartTo, DateTime? dateStartFrom,
        DateTime? dateEndTo, DateTime? dateEndFrom, string userIdFilter,
        int pageNumber = 1, int perPage = 10
        );

    /// <summary>
    /// Create a new project company user
    /// </summary>
    /// <param name="projectCompanyUser">The project company user to create</param>
    /// <param name="userId">The user who is creating the new project company user</param>
    /// <returns>The project company user created.</returns>
    Task<Result<ProjectCompanyUser>> CreateProjectCompanyUserAsync(ProjectCompanyUser projectCompanyUser, string userId);

    /// <summary>
    /// Update the status of one specifc CompanyUser in a project
    /// </summary>
    /// <param name="projectId">The id of the project.</param>
    /// <param name="companyuserId">The id of the user company.</param>
    /// <param name="userId">The user who is going to update the record.</param>
    /// <returns>The project company user updated.</returns>
    Task<Result<ProjectCompanyUser>> UpdateProjectCompanyUserStatusAsync(int projectId, int companyuserId, string userId);

    /// <summary>
    /// Obtains the projects only by companyId without paging
    /// </summary>
    /// <param name="companyId">The id of the company owning the project.</param>
    /// <returns>All records resulting from projects.</returns>
    Task<Result<List<Project>>> FindProjectsByCompanyIdAsync(int companyId, string userId);

    Task<Result<PaginatedRecord<UsersByCompanyDto>>> GetUsersByCompanyId(int companyId, int projectId, int perPage, int pageNumber);

    /// <summary>
    /// Delete the one specifc Project
    /// </summary>
    /// <param name="projectId">The id of the project.</param>
    /// <param name="userId">The user who is going to delete the record.</param>
    /// <returns>The project deleted.</returns>
    Task<Result<Project>> DeleteProject(string userId, int projectId);

    /// <summary>
    /// Add new projectCompanyUsersprojectCompanyUsers
    /// </summary>
    /// <param name="companyUserId">The Id of the companyUser that will be assigned to the projects.</param>
    /// <param name="projectListIds">The list of ids of the projects that will be assigned to the user</param>
    /// <param name="userId">The user who is creating the new project company user</param>
    /// <returns>The list of projectCompanyUser added.</returns>
    Task<Result<List<ProjectCompanyUser>>> AddProjectCompanyUsersList(int companyUserId, int[] projectListIds, string userId);

    /// <summary>
    /// Update the status of a list of projectCompanyUsers
    /// </summary>
    /// <param name="companyUserId">The Id of the companyUser in the ProjectCompanyUsers.</param>
    /// <param name="projectIdLists">The list of ids of the projects whose status will be changed (Active/Desactive)</param>
    /// <param name="userId">The user who is updating the projectCompanyUser</param>
    /// <returns>The list of projectCompanyUser with status changed.</returns>
    Task<Result<List<ProjectCompanyUser>>> UpdateProjectCompanyUsersListActiveStatus(int companyUserId, int[] projectIdLists, string userId);

    /// <summary>
    /// Create a list of project company user registers
    /// </summary>
    /// <param name="projectId">The projectID.</param>
    /// <param name="companyUserIdsList">The list of ids of the company users.</param>
    /// <param name="userId">The user who is creating a projectCompanyUser list<</param>
    /// <returns>The list of created project company users.</returns>
    Task<Result<List<ProjectCompanyUser>>> AddProjectCompanyUsersListByCompanyUserIds(int projectId, int[] companyUserIdsList, string userId);

    /// <summary>
    /// Update the status of a list of company users for a project
    /// </summary>
    /// <param name="projectId">The project ID.</param>
    /// <param name="companyUserIdsList">The list of ids of the company users whose status will be changed (Active/Unactive)</param>
    /// <param name="userId">The user who is updating the projectCompanyUser list</param>
    /// <returns>The list a result status true if everything is sucessful, if not, returns an error status.</returns>
    Task<Result<NoContentResult>> UpdateCompanyUsersStatusListForProject(int projectId, int[] companyUserIdsList, string userId);
    
    /// <summary>
    /// Assign avatar to a specific project
    /// </summary>
    /// <param name="Name">The Name to the projectAvatar.</param>
    /// <param name="Description">The Description that will be assigned to the projectAvatar.</param>
    /// <param name="ProjectId">ID of the project to which the avatar is assigned.</param>
    /// <param name="AvatarImageUrl">The AvatarImageUrl that will be assigned to the projectAvatar.</param>
    /// <param name="userId">The user who is going to update the record.</param>
    /// <returns>The project updated.</returns>
    Task<Result<List<ProjectAvatar>>> AddAvatarToProjectAsync(string? Name, string? Description, int ProjectId, List<IFormFile> AvatarImage, string userId);
    
    /// <summary>
    /// Sets the value of IsArchived as true and also fills the content of the fields, ArchivedBy and ArchivedAt.
    /// </summary>
    /// <param name="userId">The id of the user making the request.</param>
    /// <param name="projectId">The project register identificator.</param>
    /// <returns>Returns a project object containing the IsArchived property as true.</returns>
    Task<Result<Project>> ArchiveProjectAsync(string userId, int projectId);

    /// <summary>
    /// Sets the value of IsArchived as false and also empty the content of the fields, ArchivedBy and ArchivedAt.
    /// </summary>
    /// <param name="userId">The id of the user making the request.</param>
    /// <param name="projectId">The project register identificator.</param>
    /// <returns>Returns a project object containing the IsArchived property as false.</returns>
    Task<Result<Project>> UnarchiveProjectAsync(string userId, int projectId);

    /// <summary>
    /// Get all the avatars that a project can have.
    /// <param name="projectId">The project register identificator.</param>
    /// </summary>
    /// <returns>Returns a list of projectAvatars.</returns>
    Task<Result<List<ProjectAvatar>>> GetProjectAvatarsAsync(int projectId);

    /// <summary>
    /// Updates partially the details of a project
    /// </summary>
    /// <param name="patchDocument">The patched document to update the entity.</param>
    /// <param name="userId">The user making the update.</param>
    /// <returns>A NoContent response indicating a success status code if everything goes well. Returns an error otherwise.</returns>
    Task<Result<NoContentResult>> PatchProjectAsync(int projectId, JsonPatchDocument<Project> patchDocument, string userId);
}
