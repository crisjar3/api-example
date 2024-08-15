using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class PutTeamMembersRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int TimeZoneId { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public int JobRoleId { get; set; }
    }
}
