using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Settings;
using NetForemost.Core.Specifications.Languages;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Settings;

public class UserSettingsService : IUserSettingsService
{
    private readonly IAsyncRepository<Entities.TimeZones.TimeZone> _timeZoneRepository;
    private readonly IAsyncRepository<UserSettings> _userSettingsRepository;
    private readonly UserManager<User> _userManager;
    private readonly IAsyncRepository<Language> _languageRepository;

    public UserSettingsService(IAsyncRepository<Entities.TimeZones.TimeZone> timeZoneRepository, IAsyncRepository<UserSettings> userSettingsRepository, UserManager<User> userManager, IAsyncRepository<Language> languageRepository)
    {
        _timeZoneRepository = timeZoneRepository;
        _userSettingsRepository = userSettingsRepository;
        _userManager = userManager;
        _languageRepository = languageRepository;
    }

    public async Task<Result<UserSettings>> CreateUserSettingsAsync(UserSettings userSettings)
    {
        try
        {
            //Verify that the user id exists
            var userFounded = await _userManager.FindByIdAsync(userSettings.UserId);

            if (userFounded is null) return Result<UserSettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserNotExist } });

            //verify tha Language Id exists
            var existLanguage = await _languageRepository.FirstOrDefaultAsync(new FindLanguageByIdSpecification(userSettings.LanguageId));
            
            if (existLanguage is null)
            {
                return Result<UserSettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.LanguageNotFound } });
            }

            //Verify that have not previously created a configuration.
            var existingUserSettings = await _userSettingsRepository.GetBySpecAsync(new GetUserSettingsByIdSpecification(userSettings.UserId));

            if (existingUserSettings is not null) return Result<UserSettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserSettingsExisting } });

            //Create a user settings
            await _userSettingsRepository.AddAsync(userSettings);

            //Add navigation entities
            userSettings.Language = existLanguage;

            return Result<UserSettings>.Success(userSettings);
        }
        catch (Exception ex)
        {
            return Result<UserSettings>.Error(ex.Message + ex.InnerException);
        }
    }

    public async Task<Result<UserSettings>> CreateUserSettingsAutomaticAsync(string userId)
    {
        try
        {
            //Verify that the user id exists
            var user = _userManager.FindByIdAsync(userId);

            if (user == null) return Result<UserSettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserNotExist } });

            UserSettings newUserSettings = new();

            newUserSettings.NewSettings(userId);

            //Create a user settings
            await _userSettingsRepository.AddAsync(newUserSettings);

            return Result<UserSettings>.Success(newUserSettings);
        }
        catch (Exception ex)
        {
            return Result<UserSettings>.Error(ex.Message + ex.InnerException);
        }
    }

    public async Task<Result<UserSettings>> UpdateUserSettingsAsync(UserSettings userSettings)
    {
        try
        {
            //Verify that there is a configuration created.
            var existingUserSettings = await _userSettingsRepository.GetByIdAsync(userSettings.Id);

            if (existingUserSettings is null) return Result<UserSettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserSettingsNotExist } });

            //verify tha Language Id exists
            var existLanguage = await _languageRepository.FirstOrDefaultAsync(new FindLanguageByIdSpecification(userSettings.LanguageId));
            if (existLanguage is null)
            {
                return Result<UserSettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.LanguageNotFound } });
            }

            //Update record data
            existingUserSettings.BlurScreenshots = userSettings.BlurScreenshots;
            existingUserSettings.CanEditTime = userSettings.CanEditTime;
            existingUserSettings.DeleteScreencasts = userSettings.DeleteScreencasts;
            existingUserSettings.ShowInReports = userSettings.ShowInReports;
            existingUserSettings.ScreencastsFrecuency = userSettings.ScreencastsFrecuency;
            existingUserSettings.TimeOutAfter = userSettings.TimeOutAfter;
            existingUserSettings.UpdatedAt = userSettings.UpdatedAt;
            existingUserSettings.UpdatedBy = userSettings.UpdatedBy;
            existingUserSettings.LanguageId = userSettings.LanguageId;

            await _userSettingsRepository.UpdateAsync(existingUserSettings);

            //Add navigation entities
            existingUserSettings.Language = existLanguage;

            return Result<UserSettings>.Success(existingUserSettings);
        }
        catch (Exception ex)
        {
            return Result<UserSettings>.Error(ex.Message + ex.InnerException);
        }
    }

    public async Task<Result<UserSettings>> GetUserSettingsAsync(int id)
    {
        try
        {
            var userSettings = await _userSettingsRepository.GetByIdAsync(id);

            if (userSettings is null) return Result<UserSettings>.Invalid(new() { new() { ErrorMessage = ErrorStrings.UserSettingsNotExist } });

            return Result<UserSettings>.Success(userSettings);
        }
        catch (Exception ex)
        {
            return Result<UserSettings>.Error(ex.Message + ex.InnerException);
        }
    }
}