using Ardalis.Result;
using NetForemost.Core.Dtos.Dashboard;

namespace NetForemost.Core.Interfaces.Dashboards;
public interface IDashboardService
{
    /// <summary>
    /// This service is used to provide information on the latest projects, job offers and company users that were recently created.
    /// </summary>
    /// <returns>Information for the last six weeks on the latest projects, job offers and company users that have been created recently.</returns>
    Task<Result<DashboardDto>> GetInformationForDashboardAsync();
}
