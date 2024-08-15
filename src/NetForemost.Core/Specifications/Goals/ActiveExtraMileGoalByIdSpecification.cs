using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals
{
    public class ActiveExtraMileGoalByIdSpecification : Specification<GoalExtraMile>
    {
        public ActiveExtraMileGoalByIdSpecification(int id, string userId, bool includeRelations = false)
        {
            if (includeRelations)
            {
                Query.Include(extraMileGoal => extraMileGoal.Goal).ThenInclude(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);
            }

            Query.Where(
                extraMileGoal => extraMileGoal.Goal.Owner.CompanyUser.UserId == userId
                              && extraMileGoal.Id == id
                              && extraMileGoal.ActualEndDate == null
                              && extraMileGoal.IsVoided == false
           );
        }

    }
}
