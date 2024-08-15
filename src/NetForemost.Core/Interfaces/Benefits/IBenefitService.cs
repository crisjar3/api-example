using Ardalis.Result;
using NetForemost.Core.Entities.Benefits;

namespace NetForemost.Core.Interfaces.Benefits;

public interface IBenefitService
{
    Task<Result<List<Benefit>>> FindBenefitsAsync(int companyId);
    Task<Result<Benefit>> CreateCustomBenefit(Benefit benefit, string userId);
}