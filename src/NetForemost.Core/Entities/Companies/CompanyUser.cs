using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Companies;

public class CompanyUser : BaseEntity
{
    public string UserName { get; set; }
    public string? UserImageUrl { get; set; }
    public int CompanyId { get; set; }
    public string UserId { get; set; }
    public bool IsActive { get; set; }
    public string RoleId { get; set; }
    public int? JobOfferId { get; set; }
    public int? TimeZoneId { get; set; }
    public int? JobRoleId { get; set; }

    public virtual Company Company { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    public virtual JobOffer? JobOffer { get; set; }
    public virtual TimeZones.TimeZone? TimeZone { get; set; }
    public virtual JobRole? JobRole { get; set; }

    public ICollection<ProjectCompanyUser> ProjectCompanyUsers { get; set; }
}