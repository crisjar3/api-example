using Ardalis.Result;
using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Interfaces.PriorityLevels;
using NetForemost.Core.Specifications.PriorityLevels;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.PriorityLevels;
public class PriorityLevelService : IPriorityLevelService
{
    private readonly IAsyncRepository<PriorityLevel> _priorityLevelRepository;
    private readonly IAsyncRepository<PriorityLevelTranslation> _priorityLevelTranslationRepository;

    public PriorityLevelService(IAsyncRepository<PriorityLevel> priorityLevelRepository, IAsyncRepository<PriorityLevelTranslation> priorityLevelTranslationRepository)
    {
        _priorityLevelRepository = priorityLevelRepository;
        _priorityLevelTranslationRepository = priorityLevelTranslationRepository;
    }

    public async Task<Result<List<PriorityLevel>>> FindAllAsync(int languageId)
    {
        try
        {
            //Get PriorityLevelTranslation
            var priorityLevelTranslation = await _priorityLevelTranslationRepository.ListAsync(new PriorityLevelTranslationSpecification(languageId));
            return Result.Success(priorityLevelTranslation);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}
