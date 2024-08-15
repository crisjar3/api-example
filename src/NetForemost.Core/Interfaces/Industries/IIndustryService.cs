using Ardalis.Result;
using NetForemost.Core.Entities.Industries;

namespace NetForemost.Core.Interfaces.Industries;

public interface IIndustryService
{
    Task<Result<List<Industry>>> FindIndustriesAsync();
}