using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Specifications.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Companies;

public class CompanyService : ICompanyService
{
    private readonly IAsyncRepository<City> _cityRepository;
    private readonly IAsyncRepository<Company> _companyRepository;
    private readonly IAsyncRepository<CompanyUser> _companyUserRepository;
    private readonly IAsyncRepository<Industry> _industryRepository;
    private readonly IAsyncRepository<Entities.TimeZones.TimeZone> _timeZoneRepository;
    private readonly RoleManager<Role> _roleManager;

    public CompanyService(IAsyncRepository<Company> companyRepository, IAsyncRepository<City> cityRepository, IAsyncRepository<Entities.TimeZones.TimeZone> timeZonesRepository,
        IAsyncRepository<CompanyUser> companyUserRepository, RoleManager<Role> roleManager, IAsyncRepository<Industry> industryRepository)
    {
        _companyRepository = companyRepository;
        _cityRepository = cityRepository;
        _timeZoneRepository = timeZonesRepository;
        _companyUserRepository = companyUserRepository;
        _industryRepository = industryRepository;
        _roleManager = roleManager;
    }

    public async Task<Result<Company>> CreateCompanyAsync(Company company, User user)
    {
        try
        {
            //Check if city exist
            var existcompany = await _cityRepository.AnyAsync(company.CityId);

            if (!existcompany)
                return Result<Company>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CityNotFound, ErrorCode = NameStrings.HttpError_BadRequest } });

            //Verify if TimeZone exist
            var existTimeZone = await _timeZoneRepository.AnyAsync(company.TimeZoneId);

            if (!existTimeZone)
                return Result<Company>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.TimeZoneNotExist } });

            //Verify if Industry exist
            var existIndustry = await _industryRepository.AnyAsync((int)company.IndustryId);

            if (!existIndustry)
                return Result<Company>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.IndustryNotExist } });

            //Set default values
            company.CreatedAt = DateTime.UtcNow;
            company.CreatedBy = user.Id;

            //Add in database
            company = await _companyRepository.AddAsync(company);
            var role = await _roleManager.FindByNameAsync(NameStrings.RoleName_Owner);

            //Add user to owner in company
            await _companyUserRepository.AddAsync(new()
            {
                UserName = user.UserName,
                CompanyId = company.Id,
                RoleId = role.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id,
                IsActive = true,
                UserId = user.Id,
            });

            return Result<Company>.Success(company);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<Company>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Company>> FindCompanyAsync(int companyId)
    {
        try
        {
            //Get company by user id
            var company = await _companyRepository.FirstOrDefaultAsync(new GetCompanyByCompanyIdSpecification(companyId));

            //Verify if exist the company
            if (company is null)
                return Result<Company>.Error(ErrorStrings.CompanyNotFound);

            return Result<Company>.Success(company);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<Company>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<Company>> UpdateCompanyAsync(Company newCompanyData, string userId)
    {
        try
        {
            //Verify if exist company
            var findCompanyResult = await FindCompanyAsync(newCompanyData.Id);

            if (!findCompanyResult.IsSuccess)
                return Result<Company>.Error(findCompanyResult.Errors.ToArray());

            var company = findCompanyResult.Value;

            company.CityId = newCompanyData.CityId;
            company.Address1 = newCompanyData.Address1;
            company.Address2 = newCompanyData.Address2;
            company.TimeZoneId = newCompanyData.TimeZoneId;
            company.Name = newCompanyData.Name;
            company.PhoneNumber = newCompanyData.PhoneNumber;
            company.Email = newCompanyData.Email;
            company.EmployeesNumber = newCompanyData.EmployeesNumber;
            company.ZipCode = newCompanyData.ZipCode;
            company.Description = newCompanyData.Description;
            company.Website = newCompanyData.Website;
            company.IndustryId = newCompanyData.IndustryId;

            //Set update values
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdatedBy = userId;

            //Verify if exist city
            var existCity = await _cityRepository.GetByIdAsync(company.CityId);
            
            if (existCity is null)
                return Result<Company>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.CityNotFound } });

            //Verify if exist timeZone
            var existTimeZone = await _timeZoneRepository.GetByIdAsync(company.TimeZoneId);
            
            if (existTimeZone is null)
                return Result<Company>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.TimeZoneNotExist } });

            //Verify if Industry exist
            var existIndustry = await _industryRepository.GetByIdAsync(company.IndustryId);
            
            if (existIndustry is null)
                return Result<Company>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.IndustryNotExist } });

            //Update in database
            await _companyRepository.UpdateAsync(company);

            //Add navegation entities
            company.City = existCity;
            company.TimeZone = existTimeZone;
            company.Industry = existIndustry;

            return Result<Company>.Success(company);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<Company>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<bool>> AddImageCompany(string userId, string UrlImage, int companyId)
    {
        try
        {
            var findCompanyResult = await FindCompanyAsync(companyId);

            if (!findCompanyResult.IsSuccess)
                return Result<bool>.Error(findCompanyResult.Errors.ToArray());

            var company = findCompanyResult.Value;

            //Set update values
            company.UpdatedAt = DateTime.UtcNow;
            company.UpdatedBy = userId;
            company.CompanyImageUrl = UrlImage;

            await _companyRepository.UpdateAsync(company);

            return Result<bool>.Success(true);

        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }

    }

    public async Task<Result<Company>> PatchCompanyDetailsAsync(string userId, JsonPatchDocument<Company> patchCompany, int companyId)
    {
        try
        {
            Result<Company> findCompanyResult = await FindCompanyAsync(companyId);

            if (!findCompanyResult.IsSuccess)
            {
                return Result<Company>.Error(findCompanyResult.Errors.ToArray());
            }

            Company company = findCompanyResult.Value;

            Result patchValidationResult = PatchValidationHelper.ValidatePatchDocument(patchCompany);

            if (!patchValidationResult.IsSuccess)
            {
                return Result<Company>.Error(patchValidationResult.Errors.ToArray());
            }

            patchCompany.ApplyTo(company);
            await _companyRepository.SaveChangesAsync();

            return Result<Company>.Success(company);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}