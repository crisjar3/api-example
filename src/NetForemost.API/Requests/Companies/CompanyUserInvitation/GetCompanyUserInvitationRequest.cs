using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies.CompanyUserInvitation;

public class GetCompanyUserInvitationRequest : PaginationRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int CompanyId { get; set; }
}
