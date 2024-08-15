using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Dtos.TimeZone;

namespace NetForemost.Core.Dtos.Companies
{
    public class PutCompanyUserUserSettingsDto
    {
        public string UserName { get; set; }
        public RoleDto Role { get; set; }
        public TimeZoneDto TimeZone { get; set; }
        public JobRoleDto JobRole { get; set; }
    }
}
