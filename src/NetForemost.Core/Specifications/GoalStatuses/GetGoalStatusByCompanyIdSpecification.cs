using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.GoalStatuses;

public class GetGoalStatusByCompanyIdSpecification : Specification<GoalStatus, GoalStatus>
{
    public GetGoalStatusByCompanyIdSpecification(int companyId)
    {
        Query.Include(goalStatus => goalStatus.StatusCategory);
        Query.Where(goalStatus => goalStatus.CompanyId == companyId);
    }
}
