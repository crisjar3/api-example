using Ardalis.Result;
using NetForemost.Core.Dtos.TimeZone;
using NetForemost.Core.Interfaces.TimeZonesReport;
using NetForemost.Core.Queries.TimeZone;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.UserTimeLineReport
{
    public class TimeZoneReportService : ITimeZoneReportService
    {
        private readonly IQueryBuilder _builder;
        private readonly IBigQueryRepository<TimeZoneDto> _timeZoneRepository;

        public TimeZoneReportService(IQueryBuilder builder, IBigQueryRepository<TimeZoneDto> timeZoneRepository)
        {
            _builder = builder;
            _timeZoneRepository = timeZoneRepository;
        }

        public async Task<Result<IEnumerable<TimeZoneDto>>> FindAllTimeZone()
        {
            try
            {
                //Get timezone catalog
                var result = await _timeZoneRepository.ListAsync(_builder.TimeZoneCatalog());

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Error(ErrorHelper.GetExceptionError(ex));
            }
        }
    }
}
