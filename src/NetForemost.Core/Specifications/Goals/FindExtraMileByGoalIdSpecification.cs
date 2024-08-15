using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals;

public class FindExtraMileByGoalIdSpecification : Specification<GoalExtraMile>
{
    public FindExtraMileByGoalIdSpecification(int goalId)
    {
        Query.Where(goalExtraMile => goalExtraMile.GoalId == goalId);
    }
}
