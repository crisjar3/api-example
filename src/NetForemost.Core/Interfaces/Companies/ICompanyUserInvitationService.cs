using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using NetForemost.Core.Dtos.Companies.CompanyUserInvitations;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Companies;

public interface ICompanyUserInvitationService
{
    /// <summary>
    /// Service to create invitation for email to new CompanyUser
    /// </summary>
    /// <param name="newCompanyUserInvitation">companyUser invitatio to create</param> 
    /// <param name="projects"> projects to new companyUser will have access if you accept the invitation</param>
    /// <param name="userId"> user of create Invitation</param>
    /// <returns></returns>
    Task<Result<NoContentResult>> CreateInvitationCompanyUserAsync(CompanyUserInvitation newCompanyUserInvitation, IEnumerable<ProjectDtoSimple> projects, string userId);

    /// <summary>
    /// Service to get invitation for company
    /// </summary>
    /// <param name="companyId">Company to find invitation </param>
    /// <returns></returns>
    Task<Result<PaginatedRecord<CompanyUserInvitationCompleteDto>>> FindInvitationByCompanyAsync(int companyId, int perPage, int pageNumber);
}