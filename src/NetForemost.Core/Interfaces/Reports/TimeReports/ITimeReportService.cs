using Ardalis.Result;
using NetForemost.Core.Dtos.Timer;

namespace NetForemost.Core.Interfaces.Reports.TimeReports;
public interface ITimeReportService
{
    Task<Result<IEnumerable<GetBlockTimeByUserDto>>> GetBlockTimeByUserIdAndDailyEntryId(DateTime date, int companyUser, double timeZone);

    Task<Result<IEnumerable<GetMonitoringByDailyTimeBlockDto>>> GetMonitoringsByBlock(string userId, int dailyTimeBlockId);

    Task<Result<IEnumerable<GetAllTimeBlocksByUserPerDayDto>>> GetAllTimeBlocksByUserPerDay(int companyUserId, double timeZone, DateTime from, DateTime to);

    Task<Result<IEnumerable<GetDailyTimeBlockDto>>> GetProcessedTimeBlocks(int companyUserId, double timeZone, DateTime from, DateTime to);

    Task<Result<byte[]>> ExportTimeBlocksToCSV(int companyUserId, double timeZone, DateTime from, DateTime to);

    /// <summary>
    /// Build a string with a list of GetDailyTimeBlockDto to paste it in a spread sheet for daily tasks loggin purposes
    /// </summary>
    /// <param name="companyUserId">Identificator of the company user id for whom the report will be generated.</param>
    /// <param name="timeZone">Time zone identificator to get the hours worked in that specific timezone.</param>
    /// <param name="from">Starting date to generate the report</param>
    /// <param name="to">Ending date to generate the report</param>
    /// <returns>A formatted string for copying and pasting in the needed spreadsheet</returns>
    Task<Result<string>> FormatTimeBlocksForSpreadsheet(int companyUserId, double timeZone, DateTime from, DateTime to);
}