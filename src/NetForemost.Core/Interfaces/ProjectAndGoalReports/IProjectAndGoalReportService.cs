using Ardalis.Result;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Dtos.Reports.ProjectsReport;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.ProjectsAndGoalsReports
{
    public interface IProjectAndGoalReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="companyId"></param>
        /// <param name="projectIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="perPage"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        Task<Result<PaginatedRecord<ProjectGeneralInfoReportDto>>> FindProjectSummarys(IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone, int perPage, int pageNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyUsers"></param>
        /// <param name="companyId"></param>
        /// <param name="projectIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<Result<AllProjectsInfoReportDto>> FindAllProjectsSummary(IEnumerable<int> companyUsers, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goalId"></param>
        /// <returns></returns>
        Task<Result<GoalDataReportDto>> FindGoalDataById(int goalId, double timeZone);

        /// <summary>
        /// Get all goals by Company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="perPage"></param>
        /// <param name="pageNumber"></param>
        /// <returns>Return a list of goals by company id</returns>
        Task<Result<PaginatedRecord<GetProjectsNameDto>>> GetAllProjectsNameByCompanyId(int companyId, int perPage, int pageNumber);

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
        Task<Result<byte[]>> ExportProjectsSummaryReport(IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone);
    }
}
