using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Dtos.TimeZone;

namespace NetForemost.Core.Dtos.Companies
{
    public class CompanyUserSettingsDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public RoleDto Role { get; set; }
        public JobRoleDto JobRole { get; set; }
        public TimeZoneDto TimeZone { get; set; }
        public List<ProjectCompanyUserSettingsDto> ProjectsCompanyUser { get; set; }
    }
}
