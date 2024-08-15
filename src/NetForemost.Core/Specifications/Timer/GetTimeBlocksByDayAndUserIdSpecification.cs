using Ardalis.Specification;
using NetForemost.Core.Entities.Timer;

namespace NetForemost.Core.Specifications.Timer;

public class GetTimeBlocksByDayAndUserIdSpecification : Specification<DailyTimeBlock>
{
    public GetTimeBlocksByDayAndUserIdSpecification(string userId, DateTime dayInit, DateTime dayEnd)
    {
        Query.Where(
            dailyTimeBlocks => dailyTimeBlocks.Owner.CompanyUser.UserId.Equals(userId) &&
                               ((dailyTimeBlocks.TimeStart >= dayInit && dailyTimeBlocks.TimeStart <= dayEnd) ||
                               (dailyTimeBlocks.TimeEnd >= dayInit && dailyTimeBlocks.TimeEnd <= dayEnd))

        );
    }
}
