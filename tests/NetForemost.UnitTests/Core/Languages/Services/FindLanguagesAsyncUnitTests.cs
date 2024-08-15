using Moq;
using NetForemost.Core.Entities.Languages;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Languages.Services;

public class FindLanguagesAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to find Lanaguages
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindLanguagesAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var languages = new List<Language>();

        // Create the simulated service
        var languageService = ServiceUtilities.CreateLanguageService(out Mock<IAsyncRepository<Language>> languageRepository, out _);

        // Configurations for tests
        languageRepository.Setup(languageRepository => languageRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languages);

        var result = await languageService.FindLanguagesAsync();

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Verify the correct functioning of the entire process to find Lanaguages by name
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenLanguagesOrderByNameAndStateIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var languages = new List<Language>()
        {
            new Language { Id = 1, Name = "D", IsActive = true},
            new Language { Id = 2, Name = "A", IsActive = true},
            new Language { Id = 3, Name = "C", IsActive = false},
            new Language { Id = 4, Name = "B", IsActive = true},
        };

        var orderedLanguages = languages.Where(language => language.IsActive).OrderBy(language => language.Name).ToList();

        // Create the simulated service
        var languageService = ServiceUtilities.CreateLanguageService(out Mock<IAsyncRepository<Language>> languageRepository, out _);

        // Configurations for tests
        languageRepository.Setup(languageRepository => languageRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languages);

        var result = await languageService.FindLanguagesAsync();
        var isEqual = ServiceUtilities.CompareList(result.Value.ToArray(), orderedLanguages.ToArray());

        // Validations for tests
        Assert.True(result.IsSuccess && isEqual);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindLanguages_ReturnError()
    {
        // Declarations of variables
        var languages = new List<Language>();
        var testError = "Error to find languages";

        // Create the simulated service
        var languageService = ServiceUtilities.CreateLanguageService(out Mock<IAsyncRepository<Language>> languageRepository, out _);

        // Configurations for tests
        languageRepository.Setup(languageRepository => languageRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await languageService.FindLanguagesAsync();

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
