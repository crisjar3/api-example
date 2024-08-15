using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Goals;

public class GoalStatusCategory : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}
