using Ardalis.Specification;
using NetForemost.Core.Entities.ContractTypes;

namespace NetForemost.Core.Specifications.ContractTypes;
public class GetContractTypeByCompanyIdSpecification : Specification<ContractType>
{
    public GetContractTypeByCompanyIdSpecification(int companyId)
    {
        Query.Where(contractType => contractType.CompanyId == companyId || contractType.CompanyId == null);
    }
}

