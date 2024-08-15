using NetForemost.Core.Dtos.Projects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class PostCompanyUserInvitationRequest
    {
        [Required]
        [EmailAddress]
        public string EmailInvited { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int CompanyId { get; set; }
        [Required]
        public string RoleId { get; set; }
        public int? JobRoleId { get; set; }
        [Required]
        public IEnumerable<ProjectDtoSimple> Projects { get; set; }
    }
}
