using NetForemost.Core.Dtos.Account;

namespace NetForemost.Core.Dtos.Projects;
public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ProjectImageUrl { get; set; }
    public List<UserDto> UserProjects { get; set; } = new List<UserDto>();
    public List<UserDto> ProjectCompanyUsers { get; set; }
    public bool IsArchived { get; set; }
    public bool IsAccessibleForEveryone { get; set; }

    // REMOVED PROPERTIES FOR ISSUE: https://netforemost.atlassian.net/browse/NFCA-339
    // public DateTime StartedDate { get; set; }
    // public DateTime EndEstimatedDate { get; set; }
    // public float Budget { get; set; }
    // public string[] TechStack { get; set; }
    // public int CompanyId { get; set; }
    // public int EstimatedSize { get; set; }
}
