using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyService;

public class CreateCompanyTest
{
    /// <summary>
    /// Verify the correct funtioning of the entire proccess to Create Company
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration of Variables
        User user = new();
        NetForemost.Core.Entities.TimeZones.TimeZone timeZone = new();
        Company company = new()
        {
            IndustryId = 1
        };

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
             out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<Industry>> industryRepository);

        //Configuration For test
        cityRepository.Setup(cityRepository => cityRepository.AnyAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.AnyAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        industryRepository.Setup(industryRepository => industryRepository.AnyAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(true);

        companyRepository.Setup(companyRepository => companyRepository.AddAsync(
            It.IsAny<Company>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        roleManager.Setup(roleManager => roleManager.FindByNameAsync(
            It.IsAny<string>()
            )).ReturnsAsync(new Role());

        var result = await companyService.CreateCompanyAsync(company, user);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Verify if city exist
    /// </summary>
    /// <returns>Error City Not Found and HttpError BadRequest </returns>
    [Fact]
    public async Task WhenCityNotFound_ReturnErrorCityNotFound()
    {
        //Declaration of Variables
        Company company = new();
        User user = new();

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
             out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _);

        //Configuration For test
        cityRepository.Setup(cityRepository => cityRepository.
            GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((City)null);

        var result = await companyService.CreateCompanyAsync(company, user);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CityNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Verify of timezone exist
    /// </summary>
    /// <returns>error Time Zone Not Exist</returns>
    [Fact]
    public async Task WhenTimeZoneNotExist_ReturnErrorTimeZoneNotExist()
    {
        //Declaration of Variables

        Company company = new()
        {
            IndustryId = 1
        };
        User user = new();

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
             out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out _);

        //Configuration For test
        cityRepository.Setup(cityRepository => cityRepository.
            AnyAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        timeZoneRepository.Setup(
            timeZoneRepository => timeZoneRepository.AnyAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await companyService.CreateCompanyAsync(company, user);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TimeZoneNotExist, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Verify of not industry exist
    /// </summary>
    /// <returns>Error Industry Not Exist</returns>
    [Fact]
    public async Task WhenIndustryNotExist_ReturnErrorIndustryNotExist()
    {
        //Declaration of Variables
        Company company = new()
        {
            IndustryId = 1
        };
        User user = new();
        NetForemost.Core.Entities.TimeZones.TimeZone timeZone = new();

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
             out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<Industry>> industryRepository);

        //Configuration For test
        cityRepository.Setup(cityRepository => cityRepository.
            AnyAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        timeZoneRepository.Setup(
            timeZoneRepository => timeZoneRepository.AnyAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        industryRepository.Setup(industryRepository => industryRepository.AnyAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await companyService.CreateCompanyAsync(company, user);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.IndustryNotExist, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    ///Check if errorexception
    /// </summary>
    /// <returns>errorException</returns>
    [Fact]
    public async Task WhenAnUnexpectedError_ReturnErrorUnexpectedError()
    {
        //Declaration of Variables
        Company company = new();
        User user = new();
        var testError = "TEST ERROR";

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
             out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<Industry>> industryRepository);

        //Configuration For test
        cityRepository.Setup(cityRepository => cityRepository.
            GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception(testError));

        timeZoneRepository.Setup(
            timeZoneRepository => timeZoneRepository.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception(testError));

        //Problema no esta pasando industry
        industryRepository.Setup(industryRepository => industryRepository.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception(testError));

        companyRepository.Setup(companyRepository => companyRepository.
        AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>())).
        Throws(new Exception(testError)); ;

        var result = await companyService.CreateCompanyAsync(company, user);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}