using Ardalis.Result;
using NetForemost.Core.Entities.PriorityLevels;


namespace NetForemost.Core.Interfaces.PriorityLevels;

public interface IPriorityLevelService
{
    /// <summary>
    /// Gets all priority levels and returns them according to the specified language.
    /// </summary>
    /// <param name="languageId"></param>
    /// <returns></returns>
    Task<Result<List<PriorityLevel>>> FindAllAsync(int languageId);
}