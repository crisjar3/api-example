using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;


namespace NetForemost.Core.Specifications.Goals
{
    public class ExtraMileGoalsByGoalIdSpecification : Specification<GoalExtraMile>
    {
        public ExtraMileGoalsByGoalIdSpecification(int goalId)
        {
            Query.Where(extraMileGoal => extraMileGoal.GoalId == goalId);
        }
    }
}
