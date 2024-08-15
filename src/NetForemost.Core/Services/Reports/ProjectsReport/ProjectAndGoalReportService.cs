using Ardalis.Result;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Dtos.Reports.ProjectsReport;
using NetForemost.Core.Interfaces.ProjectsAndGoalsReports;
using NetForemost.Core.Queries.Goal;
using NetForemost.Core.Queries.Project;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Reports.ProjectsReport;

public class ProjectAndGoalReportService : IProjectAndGoalReportService
{
    private readonly IQueryBuilder _builder;
    private readonly IBigQueryRepository<ProjectGeneralInfoReportDto> _projectRepository;
    private readonly IBigQueryRepository<ProjectExportDto> _projectExportRepository;
    private readonly IBigQueryRepository<GoalDataReportDto> _goalRepository;
    private readonly IBigQueryRepository<GetProjectsNameDto> _projectNameRepository;
    private readonly IBigQueryRepository<AllProjectsInfoReportDto> _summaryRepository;

    public ProjectAndGoalReportService(IQueryBuilder builder, IBigQueryRepository<ProjectGeneralInfoReportDto> projectRepository, IBigQueryRepository<ProjectExportDto> projectExportRepository, IBigQueryRepository<GoalDataReportDto> goalRepository, IBigQueryRepository<GetProjectsNameDto> projectNameRepository, IBigQueryRepository<AllProjectsInfoReportDto> summaryRepository)
    {
        _builder = builder;
        _projectRepository = projectRepository;
        _projectExportRepository = projectExportRepository;
        _goalRepository = goalRepository;
        _projectNameRepository = projectNameRepository;
        _summaryRepository = summaryRepository;
    }

    public async Task<Result<PaginatedRecord<ProjectGeneralInfoReportDto>>> FindProjectSummarys(IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone, int perPage, int pageNumber)
    {
        try
        {
            //Get count of projectssummary
            var count = await _projectRepository.CountAsync(_builder.GetProjectsSummary(userIds, companyId, projectIds, startDate, endDate, timeZone));

            //Get project summarys Paginated
            var result = await _projectRepository.ListAsync(_builder.GetProjectsSummaryPaginated(perPage, pageNumber));

            var paginatedRecord = new PaginatedRecord<ProjectGeneralInfoReportDto>(result.ToList(), count, perPage, pageNumber);

            return Result.Success(paginatedRecord);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<GoalDataReportDto>> FindGoalDataById(int goalId, double timeZone)
    {
        try
        {
            var result = await _goalRepository.FirstOrDefault(_builder.GetGoal(goalId, timeZone));

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }

    }

    public async Task<Result<PaginatedRecord<GetProjectsNameDto>>> GetAllProjectsNameByCompanyId(int companyId, int perPage, int pageNumber)
    {
        try
        {
            //Get count of goals
            var countRecords = await _projectNameRepository.CountAsync(_builder.GetProjects(companyId));
            //Get GOals Paginated
            var projects = await _projectNameRepository.ListAsync(_builder.GetProjectsPaginated(perPage, pageNumber));

            var paginatedRecords = new PaginatedRecord<GetProjectsNameDto>(projects.ToList(), countRecords, perPage, pageNumber);

            return Result.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<AllProjectsInfoReportDto>> FindAllProjectsSummary(IEnumerable<int> companyUserIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone)
    {
        try
        {
            //Get ProjectsSummary: count of projects, count of goals, totalhours tracked
            var projectsSummary = await _summaryRepository.FirstOrDefault(_builder.ProjectsSummary(companyUserIds, companyId, projectIds, startDate, endDate, timeZone));

            return Result.Success(projectsSummary);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<byte[]>> ExportProjectsSummaryReport(IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone)
    {
        try
        {
            var userSummaryReport = await _projectExportRepository.ListAsync(_builder.GetProjectsSummaryExport(userIds, companyId, projectIds, startDate, endDate, timeZone));

            // Generate Content File
            byte[] fileContent = FileCSVHelper.GetCSVBytes<ProjectExportDto>(userSummaryReport);

            // Return File as response
            return Result.Success(fileContent);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}