using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Goals;

public class GoalStatus : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int StatusCategoryId { get; set; }
    public int CompanyId { get; set; }
    public bool IsFinalStatus { get; set; } = false;

    public virtual Company? Company { get; set; }
    public virtual GoalStatusCategory? StatusCategory { get; set; }

}