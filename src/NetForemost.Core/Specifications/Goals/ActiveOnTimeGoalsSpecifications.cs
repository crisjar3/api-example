using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals
{
    public class ActiveOnTimeGoalsSpecifications : Specification<Goal>
    {
        public ActiveOnTimeGoalsSpecifications(string userId)
        {
            Query.Include(goal => goal.Owner).ThenInclude(projectCompanyUser => projectCompanyUser.CompanyUser).ThenInclude(companyUser => companyUser.User);
            Query.Where(goal => goal.Owner.CompanyUser.UserId == userId &&
                                goal.TargetEndDate > DateTime.Now &&
                                goal.ActualEndDate == null
                        );
        }

    }
}
