using Ardalis.Result;
using NetForemost.Core.Entities.Seniorities;

namespace NetForemost.Core.Interfaces.Seniorities;

public interface ISeniorityService
{
    /// <summary>
    /// Gets all seniority records
    /// </summary>
    /// <returns>A list of seniority records</returns>
    Task<Result<List<Seniority>>> FindSenioritiesAsync();
}
