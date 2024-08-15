namespace NetForemost.Core.Dtos.Projects
{
    public class ProjectStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ProjectImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public bool IsAccessibleForEveryone { get; set; }
    }
}
