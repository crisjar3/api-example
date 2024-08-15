using Ardalis.Result;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Core.Interfaces.StoryPoints;
using NetForemost.Core.Specifications.StoryPoints;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.StoryPoints;
public class StoryPointService : IStoryPointService
{
    private readonly IAsyncRepository<StoryPoint> _storyPointsRepository;

    public StoryPointService(IAsyncRepository<StoryPoint> storyPointsRepository)
    {
        _storyPointsRepository = storyPointsRepository;
    }

    public async Task<Result<List<StoryPoint>>> FindAllAsync()
    {
        try
        {
            var storyPoints = await _storyPointsRepository.ListAsync(new FindAllStoryPointsSpecification());

            return Result.Success(storyPoints);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}
