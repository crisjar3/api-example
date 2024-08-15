using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Dtos.TimeZone;

namespace NetForemost.Core.Dtos.Companies;

public class CompanyUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int CompanyId { get; set; }
    public bool IsActive { get; set; }
    public RoleDto Role { get; set; }
    public JobRoleDto JobRole { get; set; }
    public TimeZoneDto TimeZone { get; set; }
}