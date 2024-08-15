using Ardalis.Result;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Interfaces.Benefits;
using NetForemost.Core.Specifications.Benefits;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Benefits;

public class BenefitService : IBenefitService
{
    private readonly IAsyncRepository<Benefit> _benefitRepository;
    private readonly IAsyncRepository<Company> _companyRepository;
    public BenefitService(IAsyncRepository<Benefit> benefitsRepository, IAsyncRepository<Company> companyRepository)
    {
        _benefitRepository = benefitsRepository;
        _companyRepository = companyRepository;
    }

    public async Task<Result<List<Benefit>>> FindBenefitsAsync(int companyId)
    {
        try
        {
            var company = await _companyRepository.GetByIdAsync(companyId);

            if (company is null) return Result<List<Benefit>>.Invalid(
                new()
                {
                    new()
                    {
                        ErrorCode = NameStrings.HttpError_BadRequest,
                        ErrorMessage = ErrorStrings.CompanyNotFound.Replace("[id]", companyId.ToString())
                    }
                });

            var benefits = await _benefitRepository.ListAsync(new GetBenefitsByCompanyIdSpecification(companyId));

            benefits = benefits.OrderBy(benefit => benefit.Name).ToList();

            return Result<List<Benefit>>.Success(benefits);
        }
        catch (Exception ex)
        {
            return Result<List<Benefit>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Benefit>> CreateCustomBenefit(Benefit benefit, string userId)
    {
        try
        {
            var existCompany = await _companyRepository.GetByIdAsync(benefit.CompanyId);
            
            if (existCompany is null) return Result<Benefit>.Invalid(
                new()
                {
                    new()
                    {
                        ErrorCode = NameStrings.HttpError_BadRequest,
                        ErrorMessage = ErrorStrings.CompanyNotFound.Replace("[id]", benefit.CompanyId.ToString())
                    }
                });

            benefit.CreatedAt = DateTime.UtcNow;
            benefit.CreatedBy = userId;
            benefit.IsDefault = false;

            benefit = await _benefitRepository.AddAsync(benefit);

            return Result<Benefit>.Success(benefit);
        }
        catch (Exception ex)
        {
            return Result<Benefit>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}