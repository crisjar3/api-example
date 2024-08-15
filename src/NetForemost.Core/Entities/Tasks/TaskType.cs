using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;
using NetForemost.SharedKernel.Entities;


namespace NetForemost.Core.Entities.Tasks
{
    public class TaskType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
