using Moq;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Specifications.Cities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Cities.Services;

public class FindCitiesAsyncTets
{
    /// <summary>
    /// Verify the correct functioning of the entire process.
    /// </summary>
    /// <returns>Returns success</returns>
    [Fact]
    public async Task WhenCountryIdExist_ReturnsSuccess()
    {
        //Delcarations of variables
        int countryId = 1;
        var country = new Country() { Id = countryId };
        var cities = new List<City>() { new() { Id = 1 } };

        //Create the simulated service
        var cityService = ServiceUtilities.CreateCityService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Country>> countryRepository);

        //Configurations for tests
        countryRepository.Setup(countryRepository
            => countryRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(country);

        cityRepository.Setup(cityRepository
            => cityRepository.ListAsync(It.IsAny<GetCityByCountrySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cities);

        var result = await cityService.FindCitiesAsync(countryId);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Check if the id does not exist that does not terminate the process correctly.
    /// </summary>
    /// <returns>Returns invalid error</returns>
    [Fact]
    public async Task WhenCountryIdNotExist_ReturnsError()
    {
        //Delcarations of variables
        int countryId = 1;
        var country = new Country() { Id = countryId };
        var cities = new List<City>() { new() { Id = 1 } };

        //Create the simulated service
        var cityService = ServiceUtilities.CreateCityService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Country>> countryRepository);

        //Configurations for tests
        countryRepository.Setup(countryRepository
            => countryRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Country)null);

        cityRepository.Setup(cityRepository
            => cityRepository.ListAsync(It.IsAny<GetCityByCountrySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cities);

        var result = await cityService.FindCitiesAsync(countryId);

        //Validations for tests
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// Verify that if an unexpected error occurs it is caught and does not break the process.
    /// </summary>
    /// <returns>Returns error</returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccurs_ReturnsError()
    {
        //Delcarations of variables
        int countryId = 1;
        var country = new Country() { Id = countryId };
        var cities = new List<City>() { new() { Id = 1 } };
        var testError = "TEST ERROR";

        //Create the simulated service
        var cityService = ServiceUtilities.CreateCityService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Country>> countryRepository);

        //Configurations for tests
        countryRepository.Setup(countryRepository
            => countryRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception(testError));

        cityRepository.Setup(cityRepository
            => cityRepository.ListAsync(It.IsAny<GetCityByCountrySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cities);

        var result = await cityService.FindCitiesAsync(countryId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}