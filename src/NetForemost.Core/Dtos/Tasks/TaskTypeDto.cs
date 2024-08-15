using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Dtos.Projects;

namespace NetForemost.Core.Dtos.Tasks
{
    public class TaskTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int ProjectId { get; set; }
        public ProjectDto Project { get; set; }
        public int CompanyId { get; set; }
        public CompanyDto Company { get; set; }
    }
}
