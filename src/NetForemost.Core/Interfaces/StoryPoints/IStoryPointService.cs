using Ardalis.Result;
using NetForemost.Core.Entities.StoryPoints;

namespace NetForemost.Core.Interfaces.StoryPoints
{
    public interface IStoryPointService
    {
        Task<Result<List<StoryPoint>>> FindAllAsync();
    }
}
