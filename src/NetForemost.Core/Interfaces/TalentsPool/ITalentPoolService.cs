using Ardalis.Result;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Interfaces.TalentsPool;

public interface ITalentPoolService
{
    Task<Result<PaginatedRecord<User>>> FindTalentAsync(
        int[] skillsId,
        int[] jobRolesId,
        int[] senioritiesId,
        int[] countries,
        int[] cities,
        string Email,
        bool? isActive,
        DateTime? startRegistrationDate,
        DateTime? endRegistrationDate,
        string name,
        string languages,
        int pageNumber = 1,
        int perPage = 10
    );
}
