using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals
{
    public class ActiveLateExtraMileGoalsSpecifications : Specification<GoalExtraMile>
    {
        public ActiveLateExtraMileGoalsSpecifications(string userId)
        {
            Query.Include(extraMileGoal => extraMileGoal.Goal).ThenInclude(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);
            Query.Where(extraMileGoal => extraMileGoal.Goal.Owner.CompanyUser.UserId == userId &&
                                extraMileGoal.ExtraMileTargetEndDate <= DateTime.Now &&
                                extraMileGoal.ActualEndDate == null
                        );
        }

    }
}
