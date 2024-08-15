using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals;

public class GetGoalByIdAndUserId : Specification<Goal>
{
    public GetGoalByIdAndUserId(int id, string userId)
    {
        Query.Include(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);

        Query.Where(goal => goal.Id == id && goal.Owner.CompanyUser.UserId == userId).
            Include(goal => goal.GoalStatus).
            Include(goal => goal.Project).
            Include(goal => goal.ScrumMaster).
            Include(goal => goal.StoryPoint).
            Include(goal => goal.PriorityLevel)
            ;
    }
}