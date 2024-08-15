using Ardalis.Specification;
using NetForemost.Core.Entities.Timer;

namespace NetForemost.Core.Specifications.BlockTimeTrackings;

public class GetDailyMonitoringBlockByIdAndOwnerId : Specification<DailyTimeBlock>
{
    public GetDailyMonitoringBlockByIdAndOwnerId(int blockTimeTrackingId, string ownerId)
    {
        Query.Where(blockTimeTracking => blockTimeTracking.Id == blockTimeTrackingId /*&&*/ /*blockTimeTracking.DailyEntry.OwnerId == ownerId*/)
            //.Include(blockTimeTracking => blockTimeTracking.DailyEntry)
            .Include(blockTimeTracking => blockTimeTracking.Monitorings);
    }
}

