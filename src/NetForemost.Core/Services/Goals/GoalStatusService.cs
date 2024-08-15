using Ardalis.Result;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.Core.Specifications.GoalStatuses;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Goals;

public class GoalStatusService : IGoalStatusService
{
    private readonly IAsyncRepository<GoalStatus> _goalStatusRepository;
    private readonly IAsyncRepository<Company> _companyRepository;

    public GoalStatusService(IAsyncRepository<GoalStatus> goalStatusRepository, IAsyncRepository<Company> companyRepository)
    {
        _companyRepository = companyRepository;
        _goalStatusRepository = goalStatusRepository;
    }
    public async Task<Result<List<GoalStatus>>> GetAllGoalStatusByCompany(int companyId)
    {
        try
        {
            // Verify if company exist
            var company = await _companyRepository.GetByIdAsync(companyId);
            if (company is null)
            {
                return Result<List<GoalStatus>>.Invalid(new List<ValidationError>()
            {
                new()
                {
                    ErrorMessage = ErrorStrings.CompanyNotFound
                }
            });
            }

            // List goal status by company
            var goalStatus = await _goalStatusRepository.ListAsync(new GetGoalStatusByCompanyIdSpecification(companyId));

            return Result<List<GoalStatus>>.Success(goalStatus);
        }
        catch (Exception ex)
        {
            return Result<List<GoalStatus>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}
