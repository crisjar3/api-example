using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyService;

public class UpdateCompanyAsyncTest
{
    /// <summary>
    /// Verify if company exist
    /// </summary>
    /// <returns>Company not found</returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnErrorCompanyNotFound()
    {
        //Declaration of variables
        Company newCompany = new();
        User User = new();

        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out _,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _
            );

        //Configuration for test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync((Company)null);

        var result = await companyService.UpdateCompanyAsync(newCompany, User.Id);

        //validation for test
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// Check if city exist
    /// </summary>
    /// <returns>errors City not found</returns>
    [Fact]
    public async Task WhenCityNotFound_ReturnErrorCityNotFound()
    {
        //Declaration of variables
        Company newCompany = new();
        User User = new();

        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _
            );

        //Configuration for test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(newCompany);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((City)null);

        var result = await companyService.UpdateCompanyAsync(newCompany, User.Id);

        //validation for test
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CityNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check if TimeZone exist
    /// </summary>
    /// <returns>error Time Zone Not Exist</returns>
    [Fact]
    public async Task WhenTimeZoneNotExist_ReturnErrorTimeZoneNotExist()
    {
        //Declaration of variables
        Company newCompany = new();
        User User = new();

        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out _
            );

        //Configuration for test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(newCompany);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((NetForemost.Core.Entities.TimeZones.TimeZone)null);

        var result = await companyService.UpdateCompanyAsync(newCompany, User.Id);

        //validation for test
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TimeZoneNotExist, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check if TimeZone exist
    /// </summary>
    /// <returns>error Industry Not Exist</returns>
    [Fact]
    public async Task WhenIndustryNotExist_ReturnErrorIndustryNotExist()
    {
        //Declaration of variables
        Company newCompany = new();
        User User = new();

        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out _,
            out Mock<IAsyncRepository<Industry>> industryRepository
            );

        //Configuration for test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(newCompany);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NetForemost.Core.Entities.TimeZones.TimeZone());

        industryRepository.Setup(industryRepository => industryRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync((Industry)null);


        var result = await companyService.UpdateCompanyAsync(newCompany, User.Id);

        //validation for test
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.IndustryNotExist, ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Verify the correct funtioning of the entire proccess
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration of variables
        Company newCompany = new();
        User User = new();
        NetForemost.Core.Entities.TimeZones.TimeZone timeZone = new();

        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<Industry>> industryRepository
            );

        //Configuration for test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(newCompany);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NetForemost.Core.Entities.TimeZones.TimeZone());

        industryRepository.Setup(industryRepository => industryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(new Industry());

        var result = await companyService.UpdateCompanyAsync(newCompany, User.Id);

        //validation for test
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    ///Check if errorexception
    /// </summary>
    /// <returns>error Exception</returns>
    [Fact]
    public async Task WhenUnexpectedError_ReturnErrorUnexpectedError()
    {
        //Declaration of variables
        Company newCompany = new Company();
        User User = new User();
        var testError = "TEST ERROR";

        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<Industry>> industryRepository
            );

        //Configuration for test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            Throws(new Exception(testError));

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .Throws(new Exception(testError));

        industryRepository.Setup(industryRepository => industryRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).
            Throws(new Exception(testError));

        var result = await companyService.UpdateCompanyAsync(newCompany, User.Id);

        //validation for test
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}