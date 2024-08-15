using Ardalis.Result;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Interfaces.Policies;
using NetForemost.Core.Specifications.Policies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Policies;
public class PolicyService : IPolicyService
{
    private readonly IAsyncRepository<Policy> _policyRepository;
    private readonly IAsyncRepository<Company> _companyRepository;

    public PolicyService(IAsyncRepository<Policy> policyRepository, IAsyncRepository<Company> companyRepository)
    {
        _policyRepository = policyRepository;
        _companyRepository = companyRepository;
    }

    public async Task<Result<List<Policy>>> FindPoliciesAsync(int companyId)
    {
        try
        {
            var company = await _companyRepository.GetByIdAsync(companyId);

            // Verify if company exist
            if (company is null)
            {
                return Result<List<Policy>>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CompanyNotFound } });
            }

            var policies = await _policyRepository.ListAsync(new GetPolicyByCompanyIdSpecification(companyId));

            return Result.Success(policies);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}
