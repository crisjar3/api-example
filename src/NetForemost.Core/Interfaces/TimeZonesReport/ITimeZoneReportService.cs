using Ardalis.Result;
using NetForemost.Core.Dtos.TimeZone;

namespace NetForemost.Core.Interfaces.TimeZonesReport
{
    public interface ITimeZoneReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<Result<IEnumerable<TimeZoneDto>>> FindAllTimeZone();
    }
}
