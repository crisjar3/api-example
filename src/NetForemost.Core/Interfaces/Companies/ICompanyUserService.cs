using Ardalis.Result;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Companies
{
    public interface ICompanyUserService
    {
        /// <summary>
        /// Add a new team member.
        /// </summary>
        /// <param name="userId">The id of the user who will create the record.</param>
        /// <param name="userToAddId">The id of the user who will be a member of the team.</param>
        /// <param name="companyId">The id of the company that will be part of the team.</param>
        /// <returns>The team member created.</returns>
        Task<Result<CompanyUser>> CreateTeamMemberAsync(CompanyUser newCompanyUser, string userId);

        /// <summary>
        /// Gets all members of a team (Company user)
        /// </summary>
        /// <param name="companyId">Members Company</param>
        /// <param name="TimeZoneIds">Timezone of users</param>
        /// <param name="companyUserIds">Members Id</param>
        /// <param name="from">Initial date</param>
        /// <param name="to">End Date</param>
        /// <param name="userId">Id of current user</param>
        /// <returns></returns>
        Task<Result<PaginatedRecord<UserSettingCompanyUserDto>>> FindTeamMembersAsync(int companyId, int[] TimeZoneIds, int[] companyUserIds, bool isArchived, DateTime from, DateTime to, int pageNumber, int perPage, string userId);

        /// <summary>
        /// Gets all members of a team (Company user)
        /// </summary>
        /// <param name="userId">The id of the user making the request.</param>
        /// <param name="companyUserId">The companyUser that users will be searched for.</param>
        /// <returns>All records resulting from CompanyUser.</returns>
        Task<Result<CompanyUser>> GetCompanyUserDetailsAsync(string userId, int companyUserId);

        /// <summary>
        /// Update a members of a team (Company user)
        /// </summary>
        /// <param name="newDataCompanyUser">The companyUser data that will be updated in the companyUser.</param>
        /// <param name="userId">The id of the user making the request.</param>
        /// <returns>The CompanyUser details updated.</returns>

        Task<Result<CompanyUser>> UpdateCompanyUserAsync(CompanyUser newDataCompanyUser, string userId);
        /// Gets all members of Company
        /// </summary>
        /// <param name="userId">The id of the user making the request.</param>
        /// <param name="companyId">The company that users will be searched for.</param>
        /// <param name="pageNumber">Page number to display</param>
        /// <param name="perPage">Number of records per page</param>
        /// <returns>All records resulting from CompanyUser.</returns>
        Task<Result<PaginatedRecord<CompanyUser>>> GetCompanyUsersAsync(string userId, int companyId, int pageNumber = 1, int perPage = 10);

        /// Delete a members of a team (Company user)
        /// </summary>
        /// <param name="userId">The id of the user making the request.</param>
        /// <param name="companyUserId">The companyUserId that will be deleted for.</param>
        /// <returns>The CompanyUser deleted.</returns>
        Task<Result<CompanyUser>> DeleteCompanyUserAsync(string userId, int companyUserId);

        /// <summary>
        /// Sets the value of IsArchived as true and also fills the content of the fields, ArchivedBy and ArchivedAt.
        /// </summary>
        /// <param name="userId">The id of the user making the request.</param>
        /// <param name="companyUserId">The company user register identificator.</param>
        /// <returns>Returns an object containing the IsArchived property as true.</returns>
        Task<Result<CompanyUser>> ArchiveCompanyUserAsync(string userId, int companyUserId);

        /// <summary>
        /// Sets the value of IsArchived as false and also fills the content of the fields, ArchivedBy and ArchivedAt.
        /// </summary>
        /// <param name="userId">The id of the user making the request.</param>
        /// <param name="companyUserId">The company user register identificator.</param>
        /// <returns>Returns an object containing the IsArchived property as false.</returns>
        Task<Result<CompanyUser>> UnarchiveCompanyUserAsync(string userId, int companyUserId);

        /// <summary>
        /// C
        /// </summary>
        /// <param name="companyUserId">Id of user in the company</param>
        /// <param name="patchCompanyUser">Patch to edit a parameter of the company User</param>
        /// <param name="userId">User making the change</param>
        /// <returns>Success</returns>
        Task<Result<NoContentResult>> PatchCompanyUserAsync(int companyUserId, JsonPatchDocument<CompanyUser> patchCompanyUser, string userId);

        /// <summary>
        /// Gets the list of all archived users for a company
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="perPage"></param>
        /// <returns>Returns a paginated list of archived users</returns>
        Task<Result<PaginatedRecord<GetCompanyArchivedUsersDto>>> GetArchivedCompanyUsersAsync(string userId, int companyId, int pageNumber = 1, int perPage = 10);
    }
}
