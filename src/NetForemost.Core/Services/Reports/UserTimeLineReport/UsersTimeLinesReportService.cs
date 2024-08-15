using Ardalis.Result;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.Reports.UsersReport;
using NetForemost.Core.Dtos.UserTimeLineReport;
using NetForemost.Core.Interfaces.Reports.UserTimeLineReport;
using NetForemost.Core.Queries.User;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Reports.UserTimeLineReport;

public class UsersTimeLinesReportService : IUsersTimeLineReport
{
    private readonly IQueryBuilder _builder;
    private readonly IBigQueryRepository<UserSummaryDto> _UserSummaryRepository;
    private readonly IBigQueryRepository<UserSummaryExportDto> _UserSummaryExportRepository;
    private readonly IBigQueryRepository<UserDataDto> _UserListRepository;
    private readonly IBigQueryRepository<UserTimeLineSummarysBarDto> _summaryBarData;

    public UsersTimeLinesReportService(IQueryBuilder builder, IBigQueryRepository<UserSummaryDto> userSummaryRepository, IBigQueryRepository<UserSummaryExportDto> userSummaryExportRepository, IBigQueryRepository<UserDataDto> userListRepository, IBigQueryRepository<UserTimeLineSummarysBarDto> summaryBarData)
    {
        _builder = builder;
        _UserSummaryRepository = userSummaryRepository;
        _UserSummaryExportRepository = userSummaryExportRepository;
        _UserListRepository = userListRepository;
        _summaryBarData = summaryBarData;
    }

    public async Task<Result<PaginatedRecord<UserSummaryDto>>> FindUsersSummary(IEnumerable<int> userIds, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone, int companyId, int perPage, int pageNumber)
    {
        try
        {
            //Count all usersSummary
            var count = await _UserSummaryRepository.CountAsync(_builder.GetUsersSummary(userIds, projectIds, startDate, endDate, timeZone, companyId));
            //Get usersSummary paginated list
            var result = await _UserSummaryRepository.ListAsync(_builder.GetUsersSummaryPaginated(perPage, pageNumber));

            var paginatedRecord = new PaginatedRecord<UserSummaryDto>(result.ToList(), count, perPage, pageNumber);

            return Result.Success(paginatedRecord);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<PaginatedRecord<UserDataDto>>> FindAllUsersByCompany(int companyId, int perPage, int pageNumber)
    {
        try
        {
            //Count All Users
            var count = await _UserListRepository.CountAsync(_builder.FindAllUsers(companyId));
            //Get users paginated
            var result = await _UserListRepository.ListAsync(_builder.FindAllUsersPaginated(perPage, pageNumber));

            var paginatedRecord = new PaginatedRecord<UserDataDto>(result.ToList(), count, perPage, pageNumber);

            return Result.Success(paginatedRecord);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<UserTimeLineSummarysBarDto>> FindUsersTimeLineSummarys(IEnumerable<int> companyUserIds, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, int companyId, double timeZone)
    {
        try
        {
            //Get data of usersSummary: total user, total goals , total timetraking
            var usersSummary = await _summaryBarData.FirstOrDefault(_builder.FindSummaryUsersList(companyUserIds, projectIds, startDate, endDate, companyId, timeZone));

            return Result.Success(usersSummary);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<byte[]>> ExportUsersSummaryReport(IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone)
    {
        try
        {
            var userSummaryReportRecord = await _UserSummaryExportRepository.ListAsync(_builder.GetUsersSummaryExport(userIds, projectIds, startDate, endDate, timeZone, companyId));

            // Generate Content File
            byte[] fileContent = FileCSVHelper.GetCSVBytes<UserSummaryExportDto>(userSummaryReportRecord);

            return Result.Success(fileContent);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}