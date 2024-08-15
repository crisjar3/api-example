using Ardalis.Result;
using NetForemost.Core.Entities.Skills;
using NetForemost.Core.Interfaces.Skills;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Skills;

public class SkillService : ISkillService
{
    private readonly IAsyncRepository<Skill> _skillRepository;
    public SkillService(IAsyncRepository<Skill> skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public async Task<Result<List<Skill>>> FindSkillsAsync()
    {
        try
        {
            var skills = await _skillRepository.ListAsync();

            return Result<List<Skill>>.Success(skills);
        }
        catch (Exception ex)
        {
            return Result<List<Skill>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}