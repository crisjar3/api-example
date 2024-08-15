using Ardalis.Specification;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Specifications.Generics;

namespace NetForemost.Core.Specifications.Goals;

public class GetGoalCompletedByUserIdAndDateRange : BaseSpecificationPagination<Goal, GetCompletedGoalDto>
{
    public GetGoalCompletedByUserIdAndDateRange(string userId, DateTime from, DateTime to, int pageNumber, int perPage, bool paginate, double timeZone) : base(pageNumber, perPage, paginate)
    {
        Query.Include(goal => goal.Owner).ThenInclude(projectCompanyUser => projectCompanyUser.CompanyUser).ThenInclude(companyUser => companyUser.User);
        Query.Where(goal => goal.Owner.CompanyUser.UserId == userId && goal.ActualEndDate != null && goal.ActualEndDate <= goal.TargetEndDate && goal.HasExtraMileGoal == false);

        if (from > DateTime.MinValue && to > DateTime.MinValue)
        {
            Query.Where(goal => goal.ActualEndDate >= from && goal.ActualEndDate <= to);
        }

        Query.Select(goal => new GetCompletedGoalDto
        {
            Id = goal.Id,
            Name = goal.Name,
            Description = goal.Description,
            EndDate = goal.ActualEndDate.Value.AddHours(timeZone),
            TargetEndTime = goal.TargetEndDate.AddHours(timeZone)
        });
    }
}
