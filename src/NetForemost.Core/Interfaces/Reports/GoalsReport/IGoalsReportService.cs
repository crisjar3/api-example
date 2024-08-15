using Ardalis.Result;
using NetForemost.Core.Dtos.Goals;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.Reports.GoalsReport;

public interface IGoalsReportService
{
    /// <summary>
    /// Get the list of goals associated with a project and CompanyUser ID
    /// </summary>
    /// <param name="companyUserId"></param>
    /// <param name="goalId"></param>
    /// <param name="projectId"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>Returns the list of goals by project and user</returns>
    Task<Result<PaginatedRecord<GetGoalByProjectDto>>> GetGoalsByProjectAndUserCompanyId(IEnumerable<int> companiesUsersIds, int projectId, DateTime from, DateTime to, double timeZone, int perPage, int pageNumber);
}