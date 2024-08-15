using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.TalentPool.Service;

public class FindTalentAsync_Test
{

    /// <summary>
    /// check if the whole process flows correctly
    /// </summary>
    /// <returns>Success , if all proccess is good</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration of variable
        int[] skillsId = null;
        int[] jobRolesId = null;
        int[] senioritiesId = null;
        int[] countries = null;
        int[] cities = null;
        string Email = null;
        bool? isActive = null;
        DateTime? startRegistrationDate = null;
        DateTime? endRegistrationDate = null;
        string name = null;
        string languages = null;
        int pageNumber = 1;
        int perPage = 10;
        var user1 = new User
        {
            LockoutEnd = DateTime.Now,
            Email = "Net@foremost.com",
            UserSkills = new List<NetForemost.Core.Entities.Users.UserSkill> { new() { Id = 1 } },
            Seniority = new NetForemost.Core.Entities.Seniorities.Seniority() { Id = 2 },
            JobRole = new NetForemost.Core.Entities.JobRoles.JobRole() { Id = 1 },
            UserLanguages = new List<NetForemost.Core.Entities.Languages.UserLanguage> { new() { LanguageId = 1, LanguageLevelId = 1 } },
            FirstName = "R",
            LastName = "S",
            IsActive = true,
            Registered = DateTime.Now,
            CityId = 1,
            City = new NetForemost.Core.Entities.Countries.City() { Id = 1, CountryId = 1 },
            SeniorityId = 2,
            JobRoleId = 1,
            TimeZoneId = 4
        };

        var user2 = new User
        {
            UserSkills = new List<NetForemost.Core.Entities.Users.UserSkill> { new() { Id = 1 }, new() { Id = 2 } },
            Seniority = new NetForemost.Core.Entities.Seniorities.Seniority() { Id = 2 },
            JobRole = new NetForemost.Core.Entities.JobRoles.JobRole() { Id = 1 },
            FirstName = "Jane",
            LastName = "Smith",
            IsActive = true,
            Registered = DateTime.Now,
            CityId = 1,
            SeniorityId = 2,
            JobRoleId = 1,
            TimeZoneId = 4
        };

        var user3 = new User
        {
            UserSkills = new List<NetForemost.Core.Entities.Users.UserSkill> { new() { Id = 1 }, new() { Id = 2 } },
            Seniority = new NetForemost.Core.Entities.Seniorities.Seniority() { Id = 2 },
            JobRole = new NetForemost.Core.Entities.JobRoles.JobRole() { Id = 1 },
            FirstName = "Alice",
            LastName = "Johnson",
            IsActive = true,
            Registered = DateTime.Now,
            CityId = 1,
            SeniorityId = 2,
            JobRoleId = 1,
            TimeZoneId = 4
        };

        var user4 = new User
        {
            UserSkills = new List<NetForemost.Core.Entities.Users.UserSkill> { new() { Id = 3 } },
            Seniority = new NetForemost.Core.Entities.Seniorities.Seniority() { Id = 3 },
            JobRole = new NetForemost.Core.Entities.JobRoles.JobRole() { Id = 3 },
            FirstName = "Bob",
            LastName = "Williams",
            IsActive = true,
            Registered = DateTime.Now,
            CityId = 13,
            SeniorityId = 14,
            JobRoleId = 15,
            TimeZoneId = 16
        };

        var user5 = new User
        {
            UserSkills = new List<NetForemost.Core.Entities.Users.UserSkill> { new() { Id = 3 } },
            Seniority = new NetForemost.Core.Entities.Seniorities.Seniority() { Id = 3 },
            JobRole = new NetForemost.Core.Entities.JobRoles.JobRole() { Id = 3 },
            FirstName = "Bob",
            LastName = "Williams",
            IsActive = true,
            Registered = DateTime.Now,
            CityId = 13,
            SeniorityId = 14,
            JobRoleId = 15,
            TimeZoneId = 16
        };

        // Add the users to a list or perform any other desired operations
        List<User> users = new List<User> { user1, user2, user3, user4 };

        var mock = users.AsQueryable().BuildMock();

        //Create simutated Service
        var talenPoolService = ServiceUtilities.CreateTalentPoolService(
            out Mock<UserManager<User>> userManager
            );
        userManager.Setup(userManager => userManager.Users).Returns(mock);

        //Configuration for test
        var result = await talenPoolService.FindTalentAsync(skillsId, jobRolesId, senioritiesId, countries,
                    cities, Email, isActive, startRegistrationDate, endRegistrationDate, name, languages, pageNumber, perPage);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// check if in the process, Lenguage have a correct format
    /// </summary>
    /// <returns> exception error</returns>
    [Fact]
    public async Task WhenLenguagesNotCorrectFormat_ReturnError()
    {
        //Declaration of variable
        int[] skillsId = new int[] { };
        int[] jobRolesId = new int[] { };
        int[] senioritiesId = new int[] { };
        int[] countries = new int[] { };
        int[] cities = new int[] { };
        string Email = "Net@foremost.com";
        bool? isActive = true;
        DateTime? startRegistrationDate = new DateTime();
        DateTime? endRegistrationDate = new DateTime();
        string name = "name";
        string languages = "miau";
        int pageNumber = 1;
        int perPage = 10;
        User user = new User();
        List<User> users = new();
        var mock = users.AsQueryable().BuildMock();
        //Create simulated Service
        var talenPoolService = ServiceUtilities.CreateTalentPoolService(
            out Mock<UserManager<User>> userManager
            );

        userManager.Setup(userManager => userManager.Users).Returns(mock);

        //Configuration for test
        var result = await talenPoolService.FindTalentAsync(skillsId,
            jobRolesId,
            senioritiesId,
            countries,
            cities,
            Email,
            isActive,
            startRegistrationDate,
            endRegistrationDate,
            name,
            languages,
            pageNumber,
            perPage);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), "Input string was not in a correct format.");
    }

    /// <summary>
    /// check if Unexpected Error 
    /// </summary>
    /// <returns> Unexpectederror</returns>
    [Fact]
    public async Task WhenUnexpectedError_ReturnUnexpectedError()
    {
        //Declaration of variable
        int[] skillsId = new int[] { };
        int[] jobRolesId = new int[] { };
        int[] senioritiesId = new int[] { };
        int[] countries = new int[] { };
        int[] cities = new int[] { };
        string Email = "Net@foremost.com";
        bool? isActive = true;
        DateTime? startRegistrationDate = new DateTime();
        DateTime? endRegistrationDate = new DateTime();
        string name = "name";
        string languages = "1,1";
        int pageNumber = 1;
        int perPage = 10;
        List<User> users = new();
        var errorMessage = "Test Error".AsQueryable().ToQueryString();
        //Create simulated Service
        var talenPoolService = ServiceUtilities.CreateTalentPoolService(
            out Mock<UserManager<User>> userManager
            );
        userManager.Setup(userManager => userManager.Users).Throws(new Exception(errorMessage));

        //Configuration for test
        //Validate Test
        var result = await talenPoolService.FindTalentAsync(skillsId,
            jobRolesId,
            senioritiesId,
            countries,
            cities,
            Email,
            isActive,
            startRegistrationDate,
            endRegistrationDate,
            name,
            languages,
            pageNumber,
            perPage);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}
