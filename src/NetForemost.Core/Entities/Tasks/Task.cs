using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Projects;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Tasks;

public class Task : BaseEntity
{
    public string Description { get; set; }
    public DateTime TargetEndDate { get; set; }
    public int TypeId { get; set; }
    public TaskType Type { get; set; }
    public int ProjectId { get; set; }
    public Goal? Goal { get; set; }
    public int? GoalId { get; set; }
    public Project Project { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public double TimeSpentInSecond { get; set; }
    public int? OwnerId { get; set; }
    public virtual ProjectCompanyUser? Owner { get; set; }

    public void AddTime(DateTime timeStart, DateTime timeEnd)
    {
        TimeSpentInSecond += (timeEnd - timeStart).TotalSeconds;
    }
}