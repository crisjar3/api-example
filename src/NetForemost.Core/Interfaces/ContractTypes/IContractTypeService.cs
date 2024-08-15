using Ardalis.Result;
using NetForemost.Core.Entities.ContractTypes;

namespace NetForemost.Core.Interfaces.ContractTypes;
public interface IContractTypeService
{
    Task<Result<List<ContractType>>> FindContractTypesAsync(int companyId);
}
