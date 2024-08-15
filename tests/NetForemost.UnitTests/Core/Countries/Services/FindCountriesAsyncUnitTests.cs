using Moq;
using NetForemost.Core.Entities.Countries;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Countries.Services;

public class FindCountriesAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Find all Countries
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindCountriesAsyncIsCorrect_ReturnSuccess()
    {
        // Delcarations of variables
        var countries = new List<Country>();

        // Create the simulated service
        var countryService = ServiceUtilities.CreateCountryService(out Mock<IAsyncRepository<Country>> countryRepository);

        // Configurations for tests
        countryRepository.Setup(countryRepository => countryRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(countries);

        var result = await countryService.FindCountriesAsync();

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccur_ReturnError()
    {
        // Delcarations of variables
        var countries = new List<Country>();
        var testError = "Error to find all Countries";

        // Create the simulated service
        var countryService = ServiceUtilities.CreateCountryService(out Mock<IAsyncRepository<Country>> countryRepository);

        // Configurations for tests
        countryRepository.Setup(countryRepository => countryRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await countryService.FindCountriesAsync();

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
