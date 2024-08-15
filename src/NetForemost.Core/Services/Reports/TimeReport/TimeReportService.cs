using System.Text;
using Ardalis.Result;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Interfaces.Reports.TimeReports;
using NetForemost.Core.Queries.DailyTimeBlock;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Reports.TimeReport;
public class TimeReportService : ITimeReportService
{
    private readonly IQueryBuilder _builder;
    private readonly IBigQueryRepository<GetBlockTimeByUserDto> _blockTimeRepository;
    private readonly IBigQueryRepository<GetMonitoringByDailyTimeBlockDto> _monitoringRepository;
    private readonly IBigQueryRepository<GetAllTimeBlocksByUserPerDayDto> _workHoursRepository;

    public TimeReportService(IQueryBuilder builder, IBigQueryRepository<GetBlockTimeByUserDto> blockTimeRepository, IBigQueryRepository<GetMonitoringByDailyTimeBlockDto> monitoringRepository, IBigQueryRepository<GetAllTimeBlocksByUserPerDayDto> workHoursRepository)
    {
        _builder = builder;
        _blockTimeRepository = blockTimeRepository;
        _monitoringRepository = monitoringRepository;
        _workHoursRepository = workHoursRepository;
    }

    public async Task<Result<IEnumerable<GetBlockTimeByUserDto>>> GetBlockTimeByUserIdAndDailyEntryId(DateTime date, int companyUser, double timeZone)
    {
        try
        {
            var results = await _blockTimeRepository.ListAsync(_builder.GetDailyTimeBBlockPerDate(date, companyUser, timeZone));
            results = AddNonWorkedBlocks(results.OrderBy(block => block.TimeStart), date);

            return Result.Success(results);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<IEnumerable<GetMonitoringByDailyTimeBlockDto>>> GetMonitoringsByBlock(string userId, int dailyTimeBlockId)
    {
        try
        {
            _builder.Select("dmb.keystrokes_min as KeystrokesMin, " +
                "dmb.mouse_movements_min as MouseMovementsMin," +
                "dmb.url_image as UrlImage," +
                "dmb.created_at as CreatedAt")
                .From("public.daily_monitoring_block dmb")
                .Join("public.daily_time_block dtb", "dtb.id = dmb.daily_time_block_id")
                .Join("public.daily_entry de", "de.id = dtb.daily_entry_id")
                .Where($"de.owner_id = '{userId}' and dtb.id = {dailyTimeBlockId}");

            var results = await _monitoringRepository.ListAsync(_builder);

            return Result.Success(results);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<IEnumerable<GetAllTimeBlocksByUserPerDayDto>>> GetAllTimeBlocksByUserPerDay(int companyUserId, double timeZone, DateTime from, DateTime to)
    {
        try
        {
            // Get summary of a Daily Entry and block times worked in this entry
            var blockTimeDailyByUser = await _workHoursRepository.ListJsonAsync(_builder.FindDailyTimeBlockByUserPerDay(companyUserId, timeZone, from, to));

            // Create dictionary to save timeblocks for date
            var blockTimeDictionary = blockTimeDailyByUser.ToDictionary(b => b.Date.Date, b => b);

            var blocks = GetTimeBlocks(from, to, blockTimeDictionary);

            return Result.Success(blocks);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<IEnumerable<GetDailyTimeBlockDto>>> GetProcessedTimeBlocks(int companyUserId, double timeZone, DateTime from, DateTime to)
    {
        try
        {
            var allTimeBlocksResult = await GetAllTimeBlocksByUserPerDay(companyUserId, timeZone, from, to);

            var allTimeBlocks = allTimeBlocksResult.Value;
            var processedTimeBlocks = allTimeBlocks
                .SelectMany(block => block.TimeBlocks)
                .Where(block => block.HoursWorked > 0)
                .ToList();

            return Result<IEnumerable<GetDailyTimeBlockDto>>.Success(processedTimeBlocks);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<GetDailyTimeBlockDto>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<byte[]>> ExportTimeBlocksToCSV(int companyUserId, double timeZone, DateTime from, DateTime to)
    {
        try
        {
            var processedTimeBlocksResult = await GetProcessedTimeBlocks(companyUserId, timeZone, from, to);

            var processedTimeBlocks = processedTimeBlocksResult.Value;

            // Assuming FileCSVHelper.GetCSVBytes can handle IEnumerable<GetDailyTimeBlockDto>
            byte[] fileContent = FileCSVHelper.GetCSVBytes<GetDailyTimeBlockDto>(processedTimeBlocks);

            return Result.Success(fileContent);
        }
        catch (Exception ex)
        {
            return Result<byte[]>.Error(ex.Message);
        }
    }

    public async Task<Result<string>> FormatTimeBlocksForSpreadsheet(int companyUserId, double timeZone, DateTime from, DateTime to)
    {
        Result<IEnumerable<GetDailyTimeBlockDto>> processedTimeBlocksResult = await GetProcessedTimeBlocks(
            companyUserId,
            timeZone,
            from,
            to
        );

        if(!processedTimeBlocksResult.IsSuccess)
        {
            return Result<string>.Error(string.Join(", ", processedTimeBlocksResult.Errors.ToList()));
        }

        StringBuilder sb = new StringBuilder();

        foreach (var block in processedTimeBlocksResult.Value)
        {
            sb.Append($"{block.Project}\t"); // Project
            sb.Append("\t"); // Ticket (empty)
            sb.Append($"{block.Task}\t"); // Task type
            sb.Append("\t"); // Description (empty)
            sb.Append($"{block.StartTime.ToString("yyyy-MM-dd HH:mm:ss")}\t"); // Start Time
            sb.Append($"{block.EndTime.ToString("yyyy-MM-dd HH:mm:ss")}\t"); // End Time
            sb.Append($"{block.HoursWorked}\t"); // Hours Quantity
            sb.AppendLine($"{block.HoursWorkedInLocalFormat}"); // Formatted Hours
        }

        return Result<string>.Success(sb.ToString());
    }

    # region Private Methods

    private IEnumerable<GetAllTimeBlocksByUserPerDayDto> GetTimeBlocks(
        DateTime from, DateTime to, Dictionary<DateTime, GetAllTimeBlocksByUserPerDayDto> blockTimeDictionary)
    {
        var differenceDay = to - from;

        for (int i = 0; i <= differenceDay.Days; i++)
        {
            var dayCurrent = from.AddDays(i);

            if (!blockTimeDictionary.ContainsKey(dayCurrent.Date))
            {
                yield return new GetAllTimeBlocksByUserPerDayDto() { Date = dayCurrent.Date };
            }
            else
            {
                yield return blockTimeDictionary[dayCurrent.Date];
            }
        }
    }

    private IEnumerable<GetBlockTimeByUserDto> AddNonWorkedBlocks(IEnumerable<GetBlockTimeByUserDto> blocks, DateTime date)
    {
        var dayStart = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        var dayEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

        var lastEndTime = dayStart;

        foreach (var block in blocks)
        {
            if (block.TimeStart > lastEndTime)
            {
                yield return GetBlockTimeByUserDto.BlockNonWorked(lastEndTime, block.TimeStart);
            }

            yield return block;

            lastEndTime = block.TimeEnd;
        }

        if (lastEndTime < dayEnd)
        {
            yield return GetBlockTimeByUserDto.BlockNonWorked(lastEndTime, dayEnd);
        }
    }

    #endregion
}