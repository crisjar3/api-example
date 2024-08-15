using Ardalis.Result;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Interfaces.Industries;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Industries;

public class IndustryService : IIndustryService
{
    private readonly IAsyncRepository<Industry> _industryRepository;

    public IndustryService(IAsyncRepository<Industry> industryRepository)
    {
        _industryRepository = industryRepository;
    }

    public async Task<Result<List<Industry>>> FindIndustriesAsync()
    {
        try
        {
            var seniorities = await _industryRepository.ListAsync();
            seniorities = seniorities.OrderBy(industry => industry.Name).ToList();

            return Result<List<Industry>>.Success(seniorities);
        }
        catch (Exception ex)
        {
            return Result<List<Industry>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}