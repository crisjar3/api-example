using Ardalis.Result;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.Reports.UsersReport;
using NetForemost.Core.Dtos.UserTimeLineReport;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Reports.UserTimeLineReport;

public interface IUsersTimeLineReport
{

    /// <summary>
    /// Find list of users with number of users, numbers of hours worked, number of goals worked of all users 
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="projectIds"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<Result<UserTimeLineSummarysBarDto>> FindUsersTimeLineSummarys(IEnumerable<int> userIds, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, int companyId, double timeZone);

    /// <summary>
    /// Find list of al users with summation of hours and goals of each users
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="projectIds"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="companyId"></param>
    /// <param name="perPage"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    Task<Result<PaginatedRecord<UserSummaryDto>>> FindUsersSummary(IEnumerable<int> userIds, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double TimeZone, int companyId, int perPage, int pageNumber);

    /// <summary>
    /// Find compny usersId and name of users
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="perPage"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    Task<Result<PaginatedRecord<UserDataDto>>> FindAllUsersByCompany(int companyId, int perPage, int pageNumber);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="companyId"></param>
    /// <param name="projectIds"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="timeZone"></param>
    /// <returns></returns>
    Task<Result<byte[]>> ExportUsersSummaryReport(IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone);

}
