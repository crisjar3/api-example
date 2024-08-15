using Ardalis.Result;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Timer;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Interfaces.Timer;
public interface ITimerService
{
    Task<Result<DailyMonitoringBlock>> CreateDailyMonitoringBlock(DailyMonitoringBlock blockMonitoring, string userId);

    Task<Result<DailyTimeBlock>> CreateDailyTimeBlock(User user, DailyTimeBlock blockTimeTracking);

    Task<Result<DailyEntryDto>> GetDailyEntryByDayAndUserId(string userId, DateTime date);
}
