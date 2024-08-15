using Ardalis.Result;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Interfaces.Seniorities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Seniorities;

public class SeniorityService : ISeniorityService
{
    private readonly IAsyncRepository<Seniority> _seniorityRepository;

    public SeniorityService(IAsyncRepository<Seniority> seniorityRepository)
    {
        _seniorityRepository = seniorityRepository;
    }

    public async Task<Result<List<Seniority>>> FindSenioritiesAsync()
    {
        try
        {
            var seniorities = await _seniorityRepository.ListAsync();

            return Result<List<Seniority>>.Success(seniorities);
        }
        catch (Exception ex)
        {
            return Result<List<Seniority>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}