using Ardalis.Result;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Interfaces.ContractTypes;
using NetForemost.Core.Specifications.ContractTypes;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.ContractTypes;
public class ContractTypeService : IContractTypeService
{
    private readonly IAsyncRepository<ContractType> _contractTypeRepository;
    private readonly IAsyncRepository<Company> _companyRepository;

    public ContractTypeService(IAsyncRepository<ContractType> contractTypeRepository, IAsyncRepository<Company> companyRepository)
    {
        _contractTypeRepository = contractTypeRepository;
        _companyRepository = companyRepository;
    }

    public async Task<Result<List<ContractType>>> FindContractTypesAsync(int companyId)
    {
        try
        {
            var company = await _companyRepository.GetByIdAsync(companyId);

            // Check if Company Exist
            if (company is null)
            {
                return Result<List<ContractType>>.Invalid(new()
                {
                    new() { ErrorMessage = ErrorStrings.CompanyNotFound, ErrorCode = NameStrings.HttpError_BadRequest }
                });
            }

            var contractTypes = await _contractTypeRepository.ListAsync(new GetContractTypeByCompanyIdSpecification(companyId));

            return Result.Success(contractTypes);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}
