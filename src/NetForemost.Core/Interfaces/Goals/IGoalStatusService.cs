using Ardalis.Result;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Interfaces.Goals;

public interface IGoalStatusService
{
    /// <summary>
    /// Get all the goal status created by companies
    /// </summary>
    /// <returns>Search for all goal status by companies.</returns>
    Task<Result<List<GoalStatus>>> GetAllGoalStatusByCompany(int companyId);
}
