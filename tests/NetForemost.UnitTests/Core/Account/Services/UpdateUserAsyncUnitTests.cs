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

public class UpdateUserAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Update User
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenUpdateTalentUserIsValid_ReturnSuccess()
    {
        // Declaration of variables
        var password = "newPassword";
        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var jobRole = new JobRole();
        var city = new City();
        var seniority = new Seniority();
        var skill = new Skill();
        var userSkills = new List<UserSkill>();
        var language = new Language();
        var userLanguages = new List<UserLanguage>();
        var languageLevel = new LanguageLevel();

        var currentUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
        };

        var newUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PhoneNumber = "12345678",
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
            out _,
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
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(currentUserData);

        userManager.Setup(userManager => userManager.UpdateAsync(
            It.IsAny<User>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.UpdateUserAsync(newUserData);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the username to update exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Username not available </returns>
    [Fact]
    public async Task WhenNewUsarnameExist_ReturnsUsernameNotAvailable()
    {
        // Declaration of variables
        var password = "newPassword";

        var currentUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
        };

        var otherUser = new User()
        {
            Id = "userId test 0",
            UserName = "usernameNotAvailable",
        };

        var newUserData = new User()
        {
            Id = "userId test",
            UserName = "usernameNotAvailable",
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PhoneNumber = "12345678",
            PasswordHash = password
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _, out _,
            out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(currentUserData);

        userManager.Setup(userManager => userManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(otherUser);

        userManager.Setup(userManager => userManager.UpdateAsync(
            It.IsAny<User>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.UpdateUserAsync(newUserData);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_UsernameNotAvalaible);
    }

    /// <summary>
    /// It checks if the new timezone not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Time Zone Not Found </returns>
    [Fact]
    public async Task WhenNewTimeUpdateZoneNotExist_ReturnsError()
    {
        // Declaration of variables
        var password = "newPassword";
        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone() { Id = 1 };

        var currentUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        var newUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PhoneNumber = "12345678",
            PasswordHash = password,
            TimeZone = new() { },
            TimeZoneId = 2
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            timeZone.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new NetForemost.Core.Entities.TimeZones.TimeZone());

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(currentUserData);

        userManager.Setup(userManager => userManager.UpdateAsync(
            It.IsAny<User>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.UpdateUserAsync(newUserData);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.TimeZoneNotExist);
    }

    /// <summary>
    /// It checks if the new City not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns City Not Found </returns>
    [Fact]
    public async Task WhenNewCityUpdateNotExist_ReturnCityNotFound()
    {
        // Declaration of variables
        var password = "newPassword";
        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City() { Id = 1 };
        var seniority = new Seniority();

        var currentUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PhoneNumber = "12345678",
            PasswordHash = password,
            City = city,
            CityId = city.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        var newUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PhoneNumber = "12345678",
            PasswordHash = password,
            City = new(),
            CityId = 2,
            Seniority = seniority,
            SeniorityId = seniority.Id,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository, out _, out _,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out Mock<UserManager<User>> userManager, out _
            );

        // Configuration for tests
        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(timeZone);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            city.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(currentUserData);

        userManager.Setup(userManager => userManager.UpdateAsync(
            It.IsAny<User>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.UpdateUserAsync(newUserData);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CityNotFound);
    }

    /// <summary>
    /// It checks if the new Semiority not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Semiority Not Found </returns>
    [Fact]
    public async Task WhenNewSeniorityUpdateNotExist_ReturnsSeniorityNotFound()
    {
        // Declaration of variables
        var password = "newPassword";
        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var city = new City();
        var seniority = new Seniority() { Id = 1 };

        var currentUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id,
            City = city,
            CityId = city.Id,
            Seniority = seniority,
            SeniorityId = seniority.Id
        };

        var newUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PhoneNumber = "12345678",
            PasswordHash = password,
            City = city,
            CityId = city.Id,
            Seniority = new(),
            SeniorityId = 2,
            TimeZone = timeZone,
            TimeZoneId = timeZone.Id
        };

        // Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out Mock<IAsyncRepository<City>> cityRepository, out _, out _,
            out Mock<IAsyncRepository<Seniority>> seniorityRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _, out _, out _, out Mock<UserManager<User>> userManager, out _
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

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            seniority.Id,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Seniority());

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(currentUserData);

        userManager.Setup(userManager => userManager.UpdateAsync(
            It.IsAny<User>()
            )).Returns(Task.FromResult(IdentityResult.Success));

        var result = await accountService.UpdateUserAsync(newUserData);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.SeniorityIdNotFound.Replace("[id]", currentUserData.SeniorityId.ToString()));
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccursToUpdateTalentUser_ReturnsError()
    {
        // Declaration of variables
        var password = "newPassword";
        var testError = "Error to update User";
        var timeZone = new NetForemost.Core.Entities.TimeZones.TimeZone();
        var jobRole = new JobRole();
        var city = new City();
        var seniority = new Seniority();
        var skill = new Skill();
        var userSkills = new List<UserSkill>();
        var language = new Language();
        var userLanguages = new List<UserLanguage>();
        var languageLevel = new LanguageLevel();

        var currentUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
        };

        var newUserData = new User()
        {
            Id = "userId test",
            UserName = "user_test",
            FirstName = "FN test",
            LastName = "LN test",
            Email = "test@email.com",
            PhoneNumber = "12345678",
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
        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(currentUserData);

        userManager.Setup(userManager => userManager.UpdateAsync(
            It.IsAny<User>()
            )).Throws(new Exception(testError));

        var result = await accountService.UpdateUserAsync(newUserData);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
