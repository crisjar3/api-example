using Ardalis.Result;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Interfaces.Reports.GoalsReport;
using NetForemost.Core.Queries.Goal;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Reports.GoalsReport;

public class GoalsReportService : IGoalsReportService
{
    private readonly IQueryBuilder _builder;
    private readonly IBigQueryRepository<GetGoalByProjectDto> _getGoalByProjectRepository;

    public GoalsReportService(IQueryBuilder builder, IBigQueryRepository<GetGoalByProjectDto> getGoalByProjectRepository)
    {
        _builder = builder;
        _getGoalByProjectRepository = getGoalByProjectRepository;

    }

    public async Task<Result<PaginatedRecord<GetGoalByProjectDto>>> GetGoalsByProjectAndUserCompanyId(IEnumerable<int> companiesUsersIds, int projectId, DateTime from, DateTime to, double timeZone, int perPage, int pageNumber)
    {
        try
        {
            //Get goals count
            var countRecords = await _getGoalByProjectRepository.CountAsync(_builder.GetGoal(companiesUsersIds, projectId, from, to, timeZone));
            //Get al goals paginated
            var goalsByProject = await _getGoalByProjectRepository.ListAsync(_builder.GetGoalPaginated(perPage, pageNumber));

            var paginatedRecords = new PaginatedRecord<GetGoalByProjectDto>(goalsByProject.ToList(), countRecords, perPage, pageNumber);

            return Result.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}