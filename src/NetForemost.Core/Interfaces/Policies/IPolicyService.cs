using Ardalis.Result;
using NetForemost.Core.Entities.Policies;

namespace NetForemost.Core.Interfaces.Policies;
public interface IPolicyService
{
    Task<Result<List<Policy>>> FindPoliciesAsync(int companyId);
}
