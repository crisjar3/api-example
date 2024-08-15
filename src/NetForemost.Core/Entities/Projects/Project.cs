using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Projects;

public class Project : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CompanyId { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime EndEstimatedDate { get; set; }
    public float Budget { get; set; }
    public string[] TechStack { get; set; }
    public int EstimatedSize { get; set; }
    public string? ProjectImageUrl { get; set; }
    public bool IsAccessibleForEveryone { get; set; } = false;

    public Company Company { get; set; }
    public virtual ICollection<ProjectCompanyUser> ProjectCompanyUsers { get; set; }
    public virtual ICollection<ProjectAvatar> ProjectAvatars { get; set; }
}