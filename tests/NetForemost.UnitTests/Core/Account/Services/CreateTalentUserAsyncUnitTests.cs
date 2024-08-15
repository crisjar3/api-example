using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Entities.Skills;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Email;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Account.Services;

public class CreateTalentUserAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create Talent User
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateTalentUserIsValid_ReturnSuccess()
    {
        // Declaration of variables
        var password = "newPassword";
        var tokenTest = "this is a token test";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var jobRole = new JobRole();
        var city = new City();
        var seniority = new Seniority();
        var skill = new Skill();
        var userSkills = new List<UserSkill>();
        var language = new Language();
        var userLanguages = new List<UserLanguage>();
        var languageLevel = new LanguageLevel();
        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            UserSkills = userSkills,
            UserLanguages = userLanguages,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Skill>> skillRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<Language>> languageRepository,
            out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(skill);

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        userManager.Setup(userManager => userManager.CreateAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.AddToRoleAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the email not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Email Not Found </returns>
    [Fact]
    public async Task WhenEmailExist_ReturnsEmailNotFound()
    {
        // Declaration of variables
        var password = "newPassword";

        var zone = new NetForemost.Core.Entities.TimeZones.TimeZone()
        {
            Id = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = "user_test",
            Offset = 6,
            UpdatedBy = "user_test",
            Text = "Is a timezone"
        };

        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_EmailNotAvailable);
    }

    /// <summary>
    /// It checks if the username exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Username not available </returns>
    [Fact]
    public async Task WhenUsernameExist_ReturnsUsernameNotAvailable()
    {
        // Declaration of variables
        var password = "newPassword";
        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_UsernameNotAvalaible);
    }

    /// <summary>
    /// It checks if the Timezone not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Timezone not found </returns>
    [Fact]
    public async Task WhenTimeZoneNotExistl_ReturnsTimeZoneNotFound()
    {
        // Declaration of variables
        var password = "newPassword";
        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password
        };

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out _, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            timeZone.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(user.TimeZone);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.TimeZoneNotExist);
    }

    /// <summary>
    /// It checks if the City not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns City not found </returns>
    [Fact]
    public async Task WhenCityNotExist_ReturnsCityNotFound()
    {
        // Declaration of variables
        var password = "newPassword";
        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out _, out _, out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out _, out _
             );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            city.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(user.City);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CityNotFound);
    }

    /// <summary>
    /// It checks if the Job Role not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Job Role not found </returns>
    [Fact]
    public async Task WhenJobRoleNotExist_ReturnsJobRoleNotFound()
    {
        // Declaration of variables
        var password = "newPassword";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var jobRole = new JobRole();
        var jobRoleExist = new JobRole();
        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out _, out Mock<IAsyncRepository<JobRole>> jobRoleRepository, out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out _, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            jobRoleExist.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(user.JobRole);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobRoleIdNotFound.Replace("[id]", user.JobRoleId.ToString()));
    }

    /// <summary>
    /// It checks if the Seniority not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Seniority not found </returns>
    [Fact]
    public async Task WhenSeniorityNotExist_ReturnsSeniorityNotFound()
    {
        // Declaration of variables
        var password = "newPassword";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var seniorityExist = new Seniority();

        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out _, out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out _, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            seniorityExist.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(user.Seniority);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.SeniorityIdNotFound.Replace("[id]", user.SeniorityId.ToString()));
    }

    /// <summary>
    /// It checks if the Skill not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Skill not found </returns>
    [Fact]
    public async Task WhenUserSkillNotExist_ReturnsUserSkillNotFound()
    {
        // Declaration of variables
        var password = "newPassword";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var userSkill = new UserSkill();
        var userSkills = new List<UserSkill>() { userSkill };
        var skillExist = new Skill() { Id = 1 };

        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            UserSkills = userSkills
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Skill>> skillRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out _, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
            skillExist.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Skill());

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.SkillIdNotFound.Replace("[id]", userSkill.SkillId.ToString()));
    }

    /// <summary>
    /// It checks if the Language not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Language not found </returns>
    [Fact]
    public async Task WhenLanguageNotExist_ReturnsLanguageNotFound()
    {
        // Declaration of variables
        var password = "newPassword";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var userSkills = new List<UserSkill>();
        var skill = new Skill();
        var userLanguages = new List<UserLanguage>();
        var language = new Language();
        var languageExist = new Language() { Id = 1 };
        var languageLevel = new LanguageLevel();
        var userLanguage = new UserLanguage() { Language = language, LanguageLevel = languageLevel };

        userLanguages.Add(userLanguage);
        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            UserSkills = userSkills,
            UserLanguages = userLanguages
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Skill>> skillRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<Language>> languageRepository,
            out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
            out _,
            out _, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(skill);

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
                languageExist.Id,
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.LanguageNotFound.Replace("[id]", userLanguage.LanguageId.ToString()));
    }

    /// <summary>
    /// It checks if the Language level not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Language level not found </returns>
    [Fact]
    public async Task WhenLanguageLevelIsInvalid_ReturnsLanguageLevelNotFound()
    {
        // Declaration of variables
        var password = "newPassword";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var userSkills = new List<UserSkill>();
        var skill = new Skill();
        var userLanguages = new List<UserLanguage>();
        var language = new Language() { Id = 1 };
        var languageLevel = new LanguageLevel() { Id = 1 };
        var languageLevelValid = new LanguageLevel() { Id = 2 };
        var userLanguage = new UserLanguage() { Language = language, LanguageLevel = languageLevel };

        userLanguages.Add(userLanguage);

        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            UserSkills = userSkills,
            UserLanguages = userLanguages
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Skill>> skillRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<Language>> languageRepository,
            out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
            out _, out _, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(skill);

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            languageLevelValid.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.LanguageLevelNotFound.Replace("[id]", userLanguage.LanguageLevelId.ToString()));
    }

    /// <summary>
    /// It checks if the Email token not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Email token not Encoded </returns>
    [Fact]
    public async Task WhenGenerateEmailTokenConfirmeIsInvalid_ReturnsEmailTokenNotEncoded()
    {
        // Declaration of variables
        var password = "newPassword";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var userSkills = new List<UserSkill>();
        var skill = new Skill();
        var userLanguages = new List<UserLanguage>();
        var language = new Language();
        var languageLevel = new LanguageLevel();

        userLanguages.Add(new UserLanguage() { Language = language, LanguageLevel = languageLevel });

        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            UserSkills = userSkills,
            UserLanguages = userLanguages,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Skill>> skillRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<Language>> languageRepository,
            out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(skill);

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        userManager.Setup(userManager => userManager.CreateAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.AddToRoleAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync((string)null);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccurs_ReturnsError()
    {
        // Declaration of variables
        var password = "newPassword";
        var tokenTest = "this is a token test";
        var testError = "Error to send EmailConfirmUser";

        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var jobRole = new JobRole();
        var city = new City();
        var seniority = new Seniority();
        var skill = new Skill();
        var userSkills = new List<UserSkill>();
        var language = new Language();
        var userLanguages = new List<UserLanguage>();
        var languageLevel = new LanguageLevel();
        var user = new User()
        {
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PasswordHash = password,
            City = city,
            CityId = city.Id,
            JobRole = jobRole,
            JobRoleId = jobRole.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            UserSkills = userSkills,
            UserLanguages = userLanguages,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Skill>> skillRepository,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<Language>> languageRepository,
            out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        {
            timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

            cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(city);

            jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

            seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(seniority);

            skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(skill);

            languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(language);

            languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(languageLevel);
        }
        userManager.Setup(userManager => userManager.CreateAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.AddToRoleAsync(
            It.IsAny<User>(),
            It.IsAny<string>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).Throws(new Exception(testError));

        var result = await accountService.CreateTalentUserAsync(user, password);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
