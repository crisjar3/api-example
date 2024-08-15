using Ardalis.Result;
using Microsoft.AspNetCore.JsonPatch;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Specifications.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Companies;

public class CompanySettingsService : ICompanySettingsService
{
    private readonly ICompanyService _companyService;
    private readonly IAsyncRepository<CompanySettings> _companySettingsRepository;

    public CompanySettingsService(ICompanyService companyService, IAsyncRepository<CompanySettings> companySettingsRepository)
    {
        _companyService = companyService;
        _companySettingsRepository = companySettingsRepository;
    }

    public async Task<Result<CompanySettings>> CreateCompanySettingsAsync(CompanySettings companySettings, string userId, int companyId)
    {
        try
        {
            //Verify that the user has a company created
            var existCompany = await _companyService.FindCompanyAsync(companyId);
            
            if (!existCompany.IsSuccess || existCompany.Value is null) return Result<CompanySettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CompanyNotRegistered } });
            
            //Verify that you do not have a previously created configuration
            var searchResult = await FindCompanySettingsAsync(userId);
            
            if (searchResult.IsSuccess) return Result<CompanySettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.OneSettingsForCompany } });
            
            //Set create values
            companySettings.CompanyId = existCompany.Value.Id;
            companySettings.CreatedAt = DateTime.UtcNow;
            companySettings.CreatedBy = userId;
            companySettings.CompanyId = companyId;
            
            //Add in database
            companySettings = await _companySettingsRepository.AddAsync(companySettings);
            
            //Add navegation entities
            companySettings.Company = existCompany.Value;

            return Result<CompanySettings>.Success(companySettings);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<CompanySettings>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<CompanySettings>> FindCompanySettingsAsync(string userId)
    {
        try
        {
            //Get company settings by user id
            var companySettings = await _companySettingsRepository.FirstOrDefaultAsync(new GetCompanySettingsByUserIdSpecification(userId));

            //Verify if exist configuration for the company
            if (companySettings is null) return Result<CompanySettings>.NotFound(ErrorStrings.CompanySettingsNotFound);

            return Result<CompanySettings>.Success(companySettings);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<CompanySettings>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<CompanySettings>> PatchCompanySettingsAsync(JsonPatchDocument<CompanySettings> patchDocument, string userId)
    {
        //Get company settings by user id
        Result<CompanySettings> companySettingsResult = await FindCompanySettingsAsync(userId);

        //Verify if exist configuration for the company
        if (!companySettingsResult.IsSuccess) 
            return Result<CompanySettings>.
                Invalid( 
                    new() { new() { ErrorMessage = ErrorStrings.CompanySettingsNotFound } }
                );

        CompanySettings companySettings = companySettingsResult.Value;

        Result patchValidationResult = PatchValidationHelper.ValidatePatchDocument(patchDocument);

        if(!patchValidationResult.IsSuccess)
        {
            return Result<CompanySettings>.Error(patchValidationResult.Errors.ToArray());
        }

        patchDocument.ApplyTo(companySettings);
        await _companySettingsRepository.SaveChangesAsync();

        return Result<CompanySettings>.Success(companySettings);
    }

    public async Task<Result<CompanySettings>> UpdateCompanySettingsAsync(CompanySettings newCompanySettings, string userId)
    {
        try
        {
            //Get company settings by user id
            var companySettingsResult = await FindCompanySettingsAsync(userId);

            //Verify if exist configuration for the company
            if (!companySettingsResult.IsSuccess) return Result<CompanySettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CompanySettingsNotFound } });

            var companySettings = companySettingsResult.Value;

            //Set new company settings data
            companySettings.FirstDayOfWeek = newCompanySettings.FirstDayOfWeek;
            companySettings.TrackingType = newCompanySettings.TrackingType;
            companySettings.BlurScreenshots = newCompanySettings.BlurScreenshots;
            companySettings.DontTimeOutOnCalls = newCompanySettings.DontTimeOutOnCalls;
            companySettings.AutoStartTracking = newCompanySettings.AutoStartTracking;
            companySettings.UserProjectsTasks = newCompanySettings.UserProjectsTasks;
            companySettings.AllowManagersCreateProjects = newCompanySettings.AllowManagersCreateProjects;
            companySettings.AllowManagersInviteNewUsers = newCompanySettings.AllowManagersInviteNewUsers;
            companySettings.AllowManagersSetUpWorkSchedules = newCompanySettings.AllowManagersSetUpWorkSchedules;

            //Set update values
            companySettings.UpdatedAt = DateTime.UtcNow;
            companySettings.UpdatedBy = userId;

            //Update in database
            await _companySettingsRepository.UpdateAsync(companySettings);

            return Result<CompanySettings>.Success(companySettings);
        }
        catch (Exception ex)
        {
            //Catch the exception
            return Result<CompanySettings>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}
