using Ardalis.Result;
using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Entities.Skills;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Extensions;
using NetForemost.Core.Interfaces.Account;
using NetForemost.Core.Interfaces.Email;
using NetForemost.Core.Interfaces.Settings;
using NetForemost.Core.Specifications.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Account;

public class AccountService : IAccountService
{
    private readonly IAsyncRepository<City> _cityRepository;
    private readonly IAsyncRepository<Skill> _skillRepository;
    private readonly IAsyncRepository<JobRole> _jobRoleRepository;
    private readonly IAsyncRepository<Seniority> _seniorityRepository;
    private readonly IAsyncRepository<Entities.TimeZones.TimeZone> _timeZoneRepository;
    private readonly IAsyncRepository<Language> _languageRepository;
    private readonly IAsyncRepository<LanguageLevel> _languageLevelRepository;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly IAsyncRepository<UserSettings> _userSettingsRepository;
    private readonly IUserSettingsService _userSettingsService;
    private readonly RoleManager<Role> _roleManager;

    public AccountService(IAsyncRepository<City> cityRepository, IAsyncRepository<Skill> skillRepository, IAsyncRepository<JobRole> jobRoleRepository,
        IAsyncRepository<Seniority> seniorityRepository, IAsyncRepository<Entities.TimeZones.TimeZone> timeZoneRepository,
        IAsyncRepository<Language> languageRepository, IAsyncRepository<LanguageLevel> languageLevelRepository, IEmailService emailService, UserManager<User> userManager, IAsyncRepository<UserSettings> userSettingsRepository, IUserSettingsService userSettingsService
        , RoleManager<Role> roleManager
        )
    {
        _cityRepository = cityRepository;
        _skillRepository = skillRepository;
        _jobRoleRepository = jobRoleRepository;
        _seniorityRepository = seniorityRepository;
        _timeZoneRepository = timeZoneRepository;
        _languageRepository = languageRepository;
        _languageLevelRepository = languageLevelRepository;
        _emailService = emailService;
        _userManager = userManager;
        _userSettingsRepository = userSettingsRepository;
        _userSettingsService = userSettingsService;
        _roleManager = roleManager;
    }

    public async Task<Result<User>> CreateHireUser(User user, string password)
    {
        try
        {
            //If no username is sent create a random one
            user.UserName ??= user.FirstName.Trim().ToLower() + user.LastName.Trim().ToLower() + DateTime.UtcNow.ToString("yyyy.MM.dd.H.mm.ss.ff").Replace(".", "");

            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = true;
            user.IsActive = true;
            user.Registered = DateTime.UtcNow;

            //Verify that there is no other user with the same email
            var existEmail = await _userManager.FindByEmailAsync(user.Email);

            if (existEmail is not null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = string.Format(ErrorStrings.Result_EmailNotAvailable, user.Email)
                    }
                });

            //Verify that there is no other user with the same username
            var existUser = await _userManager.FindByNameAsync(user.UserName);

            if (existUser is not null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.Result_UsernameNotAvalaible
                    }
                });

            //Verify if exist city when the cityId is not null
            if (user.CityId is not null && user.CityId > 0)
            {
                var existCity = await _cityRepository.GetByIdAsync(user.CityId);

                if (existCity is null) return Result<User>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage=ErrorStrings.CityNotFound }
                });
            }
            else
            {
                user.CityId = null;
            }

            //Create a new user
            var resultCreateUser = await _userManager.CreateAsync(user, password);

            if (resultCreateUser.Succeeded)
            {
                //Create User Settings
                await _userSettingsService.CreateUserSettingsAutomaticAsync(user.Id);

                //Assigning the role to the user created in the database
                var resultRoleAssignment = await _userManager.AddToRoleAsync(user, NameStrings.RoleName_Owner);

                if (resultRoleAssignment.Succeeded)
                {
                    //Create email verification code
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = Base64Helper.Encode(code);

                    //Send account creation successful email and email verification

                    var link = RouteStrings.Url_ConfirmEmail.Replace("[userId]", user.Id).Replace("[token]", code).Replace("[Url_HostApp]", RouteStrings.Url_HostApp);

                    var trySendConfirmEmail = await _emailService.TrySendEmail(user.Email, HtmlTemplatesStrings.ConfirmEmail,
                        HtmlTemplates.EmailWithButton
                            .Replace("[MainText]", HtmlTemplatesStrings.ConfirmAccountMainText)
                            .Replace("[ButtonText]", HtmlTemplatesStrings.ConfirmAccountButtonText)
                            .Replace("[ButtonLink]", link)
                            .Replace("[HelpText]", HtmlTemplatesStrings.EmailHelpText)
                            .Replace("[ClosingText]", HtmlTemplatesStrings.EmailClosingText), true);

                    //Endpoint response
                    return trySendConfirmEmail.IsSuccess ? Result<User>.Success(user) : Result<User>.Error(trySendConfirmEmail.Errors.ToArray());
                }

                return Result<User>.Error(string.Join(",", resultRoleAssignment.Errors.Select(error => error.Description)));
            }

            return Result<User>.Invalid(
                new List<ValidationError>
                {
                    new() { ErrorMessage = string.Join(",", resultCreateUser.Errors.Select(error => error.Description)) }
                });
        }
        catch (Exception ex)
        {
            return Result<User>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<User>> CreateTalentUserAsync(User user, string password)
    {
        try
        {
            //If no username is sent create a random one
            user.UserName ??= user.FirstName.Trim().ToLower() + user.LastName.Trim().ToLower() + DateTime.UtcNow.ToString("yyyy.MM.dd.H.mm.ss.ff").Replace(".", "");

            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = true;
            user.IsActive = false;
            user.Registered = DateTime.UtcNow;

            //Verify that there is no other user with the same email
            var existEmail = await _userManager.FindByEmailAsync(user.Email);

            if (existEmail is not null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = string.Format(ErrorStrings.Result_EmailNotAvailable, user.Email)
                    }
                });

            //Verify that there is no other user with the same username
            var existUser = await _userManager.FindByNameAsync(user.UserName);

            if (existUser is not null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.Result_UsernameNotAvalaible
                    }
                });

            //Verify if TimeZone exist
            var existTimeZone = await _timeZoneRepository.GetByIdAsync(user.TimeZoneId);
            
            if (existTimeZone is null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage=ErrorStrings.TimeZoneNotExist }
                });

            //Verify if exist city
            var existCity = await _cityRepository.GetByIdAsync(user.CityId);
            
            if (existCity is null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage=ErrorStrings.CityNotFound }
                });

            //Verify if exist jobRole
            var existJobRole = await _jobRoleRepository.GetByIdAsync(user.JobRoleId);
            
            if (existJobRole is null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage=ErrorStrings.JobRoleIdNotFound.Replace("[id]", user.JobRoleId.ToString()) }
                });

            //Verify if exist seniority
            var existSeniority = await _seniorityRepository.GetByIdAsync(user.SeniorityId);

            if (existSeniority is null)
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage=ErrorStrings.SeniorityIdNotFound.Replace("[id]", user.SeniorityId.ToString()) }
                });

            //Verify if exist the skills
            foreach (var userSkill in user.UserSkills)
            {
                userSkill.Skill = await _skillRepository.GetByIdAsync(userSkill.SkillId);
                userSkill.CreatedBy = user.Email;
                userSkill.CreatedAt = DateTime.UtcNow;

                if (userSkill.Skill is null) return Result<User>.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.SkillIdNotFound.Replace("[id]", userSkill.SkillId.ToString())
                        }
                    });
            }

            //verify exist languages
            foreach (var userLanguage in user.UserLanguages)
            {
                userLanguage.Language = await _languageRepository.GetByIdAsync(userLanguage.LanguageId);
                userLanguage.LanguageLevel = await _languageLevelRepository.GetByIdAsync(userLanguage.LanguageLevelId);

                userLanguage.CreatedBy = user.Email;
                userLanguage.CreatedAt = DateTime.UtcNow;

                if (userLanguage.Language is null) return Result<User>.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.LanguageNotFound.Replace("[id]", userLanguage.LanguageId.ToString())
                        }
                    });

                if (userLanguage.LanguageLevel is null) return Result<User>.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.LanguageLevelNotFound.Replace("[id]", userLanguage.LanguageLevelId.ToString())
                        }
                    });
            }

            //Create a new user
            var resultCreateUser = await _userManager.CreateAsync(user, password);

            //Add navigation entities
            user.TimeZone = existTimeZone;
            user.City = existCity;
            user.JobRole = existJobRole;
            user.Seniority = existSeniority;

            if (!resultCreateUser.Succeeded)
            {
                return Result<User>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = string.Join(",", resultCreateUser.Errors.Select(error => error.Description)) }
                });
            }

            //Create email verification code
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = Base64Helper.Encode(code);

            //Send account creation successful email and email verification

            var trySendEmail = await _emailService.TrySendEmail(
                user.Email,
                HtmlTemplatesStrings.TalentAccountTittle,
                HtmlTemplates.EmailInformative
                    .Replace("[MainText]", HtmlTemplatesStrings.TalentAccountMainText)
                    .Replace("[HelpText]", HtmlTemplatesStrings.EmailHelpText)
                    .Replace("[ClosingText]", HtmlTemplatesStrings.EmailClosingText), true);

            //Create User Settings
            await _userSettingsService.CreateUserSettingsAutomaticAsync(user.Id);

            //Endpoint response
            return trySendEmail.IsSuccess ? Result<User>.Success(user) : Result<User>.Error(trySendEmail.Errors.ToArray());
        }
        catch (Exception ex)
        {
            return Result<User>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<User>> UpdateUserAsync(User userNewData)
    {
        try
        {
            var userCurrentData = await _userManager.FindByIdAsync(userNewData.Id);

            if (userNewData.UserName is not null && userNewData.UserName.ToUpper() != userCurrentData.UserName.ToUpper())
            {
                var userAvailable = await _userManager.FindByNameAsync(userNewData.UserName);

                if (userAvailable is not null)
                    return Result<User>.Invalid(new List<ValidationError>
                {
                    new ValidationError
                        {ErrorMessage = string.Format(ErrorStrings.Result_UsernameNotAvalaible, userNewData.UserName)}
                });

                await _userManager.SetUserNameAsync(userCurrentData, userNewData.UserName);
            }

            Entities.TimeZones.TimeZone? existTimeZone = null;
            //verify if the time zone exist
            if (userNewData.TimeZoneId != 0)
            {
                existTimeZone = await _timeZoneRepository.GetByIdAsync(userNewData.TimeZoneId);
                
                if (existTimeZone is null)
                    return Result<User>.Invalid(new List<ValidationError>
                    {
                        new ValidationError
                        {
                            ErrorMessage =ErrorStrings.TimeZoneNotExist
                        }
                    });
            }

            City? existCity = null;
            //Verification that the city exists
            if (userNewData.CityId != 0)
            {
                 existCity = await _cityRepository.GetByIdAsync(userNewData.CityId);

                if (existCity is null)
                    return Result<User>.Invalid(new List<ValidationError>
                    {
                        new ValidationError
                        {
                            ErrorMessage =ErrorStrings.CityNotFound
                        }
                    });
            }

            Seniority? existSeniority = null; 
            //Verification that the city exists
            if (userNewData.SeniorityId != 0)
            {
                 existSeniority = await _seniorityRepository.GetByIdAsync(userNewData.SeniorityId);

                if (existSeniority is null)
                    return Result<User>.Invalid(new List<ValidationError>
                    {
                        new ValidationError
                        {
                            ErrorMessage =ErrorStrings.SeniorityIdNotFound.Replace("[id]" , userCurrentData.SeniorityId.ToString())
                        }
                    });
            }

            //Verify that data is not empty to only update those that have value
            userCurrentData.FirstName = userNewData.FirstName is not null ? userNewData.FirstName : userCurrentData.FirstName;
            userCurrentData.LastName = userNewData.LastName is not null ? userNewData.LastName : userCurrentData.LastName;
            userCurrentData.PhoneNumber = userNewData.PhoneNumber is not null ? userNewData.PhoneNumber : userCurrentData.PhoneNumber;

            var result = await _userManager.UpdateAsync(userCurrentData);

            //Add navigation entities
            if(existTimeZone is not null)
                userNewData.TimeZone = existTimeZone;

            if(existCity is not null)
                userNewData.City = existCity;

            if (existSeniority is not null)
                userNewData.Seniority = existSeniority;

            if (result.Succeeded)
                return Result<User>.Success(userCurrentData);

            return Result<User>.Error(string.Join(",", result.Errors.Select(error => error.Description)));
        }
        catch (Exception ex)
        {
            return Result<User>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<bool>> UpdatePasswordAsync(string username, string oldPassword, string password)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, password);

            if (!result.Succeeded)
            {
                string errors = string.Join(",", result.Errors.Select(error => error.Description));

                if (errors.Contains("Passwords must be") || errors.Contains("Passwords must have"))
                {
                    return Result<bool>.Invalid(new() { new() { ErrorMessage = ErrorStrings.Result_PasswordRules } });
                }
                else if (errors.Contains("Incorrect password"))
                {
                    return Result<bool>.Invalid(new() { new() { ErrorMessage = ErrorStrings.IncorrectCurrentPassword } });
                }
                else
                {
                    return Result<bool>.Error(errors);
                }
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<Result<bool>> GenerateResetPasswordTokenAsync(string email)
    {
        try
        {
            //verify that the sent user is registered
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result<bool>.Invalid(new() { new() { ErrorMessage = ErrorStrings.Result_EmailNotFound } });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var code = Base64Helper.Encode(token);

            //Get url to reset password
            var link = RouteStrings.Url_ForgotPassword
                .Replace("[Url_HostApp]", RouteStrings.Url_HostApp)
                .Replace("[userId]", user.Id)
                .Replace("[token]", code);

            //Send email to user
            var trySendEmail = await _emailService.TrySendEmail(
                user.Email,
                HtmlTemplatesStrings.ResetPasswordTittle,
                HtmlTemplates.EmailWithButton
                    .Replace("[MainText]", HtmlTemplatesStrings.ResetPasswordMainText)
                    .Replace("[ButtonText]", HtmlTemplatesStrings.ResetPasswordButtonText)
                    .Replace("[ButtonLink]", link)
                    .Replace("[HelpText]", HtmlTemplatesStrings.EmailHelpText)
                    .Replace("[ClosingText]", HtmlTemplatesStrings.EmailClosingText), true);

            return trySendEmail.IsSuccess ? Result<bool>.Success(true) : Result<bool>.Error(trySendEmail.Errors.ToArray());
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<Result<bool>> ResetPasswordAsync(string userId, string encodedToken, string password)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            //verify that the sent user is registered
            if (user is null)
                return Result<bool>.Invalid(new List<ValidationError> {
                    new()
                    {
                        ErrorMessage = $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ResetPasswordTittle)} {ErrorStrings.Result_ReOpenFromEmail_Error}"
                    }
                });

            if (!Base64Helper.IsBase64String(encodedToken))
                return Result<bool>.Invalid(new List<ValidationError> {
                    new()
                    {
                        ErrorMessage = $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ResetPasswordTittle)} {ErrorStrings.Result_ReOpenFromEmail_Error}"
                    }
                });

            var token = Base64Helper.Decode(encodedToken);

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
            {
                string errors = string.Join(",", result.Errors.Select(error => error.Description));

                if (errors.Contains("Passwords must be") || errors.Contains("Passwords must have"))
                {
                    return Result<bool>.Invalid(new() { new() { ErrorMessage = ErrorStrings.Result_PasswordRules } });
                }

                if (errors.Contains("Invalid token"))
                {
                    await _userManager.UpdateAsync(user);
                    return Result<bool>.Error(ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ResetPasswordTittle));
                }
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<Result<bool>> GenerateConfirmEmailTokenAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result<bool>.Invalid(new() { new() { ErrorMessage = ErrorStrings.Result_EmailNotFound } });

            //Check if the user has not confirmed their email
            var isConfirm = await _userManager.IsEmailConfirmedAsync(user);

            if (isConfirm)
                return Result<bool>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.Result_VerifiedAccount } });

            //Create email verification code
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = Base64Helper.Encode(code);

            //Send account creation successful email and email verification
            var confirmEmailLink = RouteStrings.Url_ConfirmEmail
                .Replace("[userId]", user.Id)
                .Replace("[token]", code)
                .Replace("[Url_HostApp]", RouteStrings.Url_HostApp);

            var trySendConfirmEmail = await _emailService.TrySendEmail(user.Email, HtmlTemplatesStrings.ConfirmAccountTittle,
                HtmlTemplates.EmailWithButton
                .Replace("[MainText]", HtmlTemplatesStrings.ConfirmAccountMainText)
                .Replace("[ButtonText]", HtmlTemplatesStrings.ConfirmAccountButtonText)
                .Replace("[ButtonLink]", confirmEmailLink)
                .Replace("[HelpText]", HtmlTemplatesStrings.EmailHelpText)
                .Replace("[ClosingText]", HtmlTemplatesStrings.EmailClosingText), true);

            if (trySendConfirmEmail.IsSuccess) return Result<bool>.Success(true);

            //Endpoint response
            return Result<bool>.Error(trySendConfirmEmail.Errors.ToArray());
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<Result<bool>> ConfirmEmailAsync(string userId, string encodedToken)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            //verify that the sent user is registered
            if (user is null)
                return Result<bool>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ConfirmAccountTittle)}, {ErrorStrings.Result_ReOpenFromEmail_Error}" }
                });

            //Verify that the email is not already confirmed
            if (await _userManager.IsEmailConfirmedAsync(user))
                return Result<bool>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = ErrorStrings.Result_GenericCompletedSecurityProcess_Error }
                });

            //Check if the email verification code comes in base64
            if (!Base64Helper.IsBase64String(encodedToken))
                return Result<bool>.Invalid(new List<ValidationError>
                {
                    new() { ErrorMessage = $"{ErrorStrings.Result_GenericSecurity_Error.Replace("[Value]", HtmlTemplatesStrings.ConfirmAccountTittle)}, {ErrorStrings.Result_ReOpenFromEmail_Error}" }
                });

            string token = Base64Helper.Decode(encodedToken);

            //Verify the user's email
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                //Send the verification success email to the user
                var trySendConfirmedEmail = await _emailService.TrySendEmail(
                    user.Email,
                    HtmlTemplatesStrings.ConfirmAccountTittle,
                    HtmlTemplates.EmailInformative
                    .Replace("[MainText]", HtmlTemplatesStrings.ConfirmedAccountMainText)
                    .Replace("[HelpText]", HtmlTemplatesStrings.ConfirmedAccountHelpText)
                    .Replace("[ClosingText]", HtmlTemplatesStrings.EmailClosingText),
                    true);

                if (trySendConfirmedEmail.IsSuccess)
                    return Result<bool>.Success(true);

                return Result<bool>.Error(trySendConfirmedEmail.Errors.ToArray());
            }

            if (string.Join(", ", result.Errors.Select(error => error.Description).ToArray()).Contains(ErrorStrings.Validation_InvalidToken_Error))
                return Result<bool>.Invalid(new List<ValidationError> { new() { ErrorMessage = ErrorStrings.Result_RequestExpired_Error.Replace("[Value]", HtmlTemplatesStrings.ConfirmAccountTittle) } });

            return Result<bool>.Error(result.Errors.Select(error => error.Description).ToArray());
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex.Message);
        }
    }

    public async Task<Result<User>> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await _userManager.ImplementSpecification(new GetUserByIdSpecification(userId)).FirstOrDefaultAsync();

            if (user is null)
            {
                return Result.NotFound(ErrorStrings.User_NotFound);
            }

            return Result.Success(user);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
    public async Task<Result<bool>> AddImageToUserAsync(string userId, string ImageUrl)
    {
        try
        {
            var user = await _userManager.ImplementSpecification(new GetUserByIdSpecification(userId)).FirstOrDefaultAsync();
            if (user is null)
            {
                return Result.NotFound(ErrorStrings.User_NotFound);
            }

            user.UserImageUrl = ImageUrl;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return Result<bool>.Success(true);

            return Result<bool>.Error(result.Errors.Select(error => error.Description).ToArray());
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
    public async Task<Result<bool>> VarifyUserAsync(string email, string userName)
    {
        try
        {
            //Verify that there is no other user with the same email
            var existEmail = await _userManager.FindByEmailAsync(email);

            if (existEmail is not null)
                return Result<bool>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.Result_EmailNotAvailable
                    }
                });

            //Verify that there is no other user with the same username
            var existUser = await _userManager.FindByNameAsync(userName);

            if (existUser is not null)
                return Result<bool>.Invalid(new List<ValidationError>
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.Result_UsernameNotAvalaible
                    }
                });

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<Role>>> GetRolesAsync()
    {
        try
        {
            //Get Roles
            var roles = await _roleManager.Roles.ToListAsync();

            return Result<List<Role>>.Success(roles);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result<List<Role>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

}