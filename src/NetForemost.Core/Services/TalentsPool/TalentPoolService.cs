using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Extensions;
using NetForemost.Core.Interfaces.TalentsPool;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Entities;
using NetForemost.SharedKernel.Helpers;

namespace NetForemost.Core.Services.TalentsPool;

public class TalentPoolService : ITalentPoolService
{
    private readonly UserManager<User> _userManager;

    public TalentPoolService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<PaginatedRecord<User>>> FindTalentAsync(
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
    )
    {
        try
        {
            //parsing the languages from the forma 1,2:3,4
            var parsedLanguages = ParseLanguages(languages);

            //filters specification
            var filterCountSpecification = new GetUserSpecification(skillsId, jobRolesId, senioritiesId, countries, cities, Email, isActive, startRegistrationDate, endRegistrationDate, name, parsedLanguages, pageNumber, perPage, false);
            var filterSpecification = new GetUserSpecification(skillsId, jobRolesId, senioritiesId, countries, cities, Email, isActive, startRegistrationDate, endRegistrationDate, name, parsedLanguages, pageNumber, perPage, true);

            //records
            var count = await _userManager.ImplementSpecification(filterCountSpecification).CountAsync();
            var users = await _userManager.ImplementSpecification(filterSpecification).ToListAsync();

            //Paginate result
            var paginatedRecords = new PaginatedRecord<User>(users, count, perPage, pageNumber);

            return Result.Success(paginatedRecords);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    private Dictionary<int, int> ParseLanguages(string input)
    {
        var result = new Dictionary<int, int>();


        if (input is not null)
        {
            string[] pairs = input.Split(':');

            foreach (string pair in pairs)
            {
                int[] values = Array.ConvertAll(pair.Split(','), int.Parse);

                if (!result.ContainsKey(values[0]))
                {
                    result.Add(values[0], values[1]);
                }
            }
        }

        return result;
    }
}
