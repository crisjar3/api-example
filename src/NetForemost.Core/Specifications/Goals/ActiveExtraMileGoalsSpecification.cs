using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals
{
    public class ActiveExtraMileGoalsSpecification : Specification<GoalExtraMile, GoalExtraMile>
    {
        public ActiveExtraMileGoalsSpecification(string userId, double timeZone)
        {

            Query.Include(extraMileGoal => extraMileGoal.Goal).ThenInclude(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);
            Query.Include(extraMileGoal => extraMileGoal.Goal.ScrumMaster);
            Query.Include(extraMileGoal => extraMileGoal.GoalStatus).ThenInclude(goalStatus => goalStatus.StatusCategory);

            Query.Where(extraMileGoal => extraMileGoal.Goal.Owner.CompanyUser.UserId == userId &&
                                         extraMileGoal.ActualEndDate == null &&
                                         extraMileGoal.IsVoided == false
                       );
            Query.Select(goalExtraMile => new GoalExtraMile()
            {
                Id = goalExtraMile.Id,
                ExtraMileTargetEndDate = goalExtraMile.ExtraMileTargetEndDate.AddHours(timeZone),
                ActualEndDate = goalExtraMile.ActualEndDate.Value.AddHours(timeZone),
                GoalId = goalExtraMile.GoalId,
                Goal = goalExtraMile.Goal,
                IsVoided = goalExtraMile.IsVoided,
                IsActive = goalExtraMile.IsActive,
                GoalStatusId = goalExtraMile.GoalStatusId,
                GoalStatus = goalExtraMile.GoalStatus
            });


            Query.OrderBy(extraMileGoal => extraMileGoal.ExtraMileTargetEndDate);

        }
    }
}
