using Ardalis.Result;
using NetForemost.Core.Entities.Skills;

namespace NetForemost.Core.Interfaces.Skills;

public interface ISkillService
{
    Task<Result<List<Skill>>> FindSkillsAsync();
}
