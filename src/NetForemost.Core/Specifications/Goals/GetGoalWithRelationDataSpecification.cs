using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals;

public class GetGoalWithRelationDataSpecification : Specification<Goal>
{
    public GetGoalWithRelationDataSpecification(int? goalId)
    {
        Query.Include(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);

        Query.Where(goal => goal.Id == goalId);
    }
}
