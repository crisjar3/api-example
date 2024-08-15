using Moq;
using NetForemost.Core.Entities.Languages;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Languages.Services;

public class FindLanguageLevelsAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to find Languages level
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindLanguagesLevelAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var languagesLevel = new List<LanguageLevel>();

        // Create the simulated service
        var languageService = ServiceUtilities.CreateLanguageService(out _, out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository);

        // Configurations for tests
        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languagesLevel);

        var result = await languageService.FindLanguageLevelsAsync();

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Verify the correct functioning of the entire process to find languages level order by name
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenLanguagesLevelOrderByNameAndStateIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var languagesLevel = new List<LanguageLevel>()
        {
            new LanguageLevel { Id = 1, Name = "D"},
            new LanguageLevel { Id = 2, Name = "A"},
            new LanguageLevel { Id = 3, Name = "C"},
            new LanguageLevel { Id = 4, Name = "B"},
        };

        var orderedLanguagesLevel = languagesLevel.OrderBy(language => language.Name).ToList();

        // Create the simulated service
        var languageService = ServiceUtilities.CreateLanguageService(out _, out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository);

        // Configurations for tests
        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languagesLevel);

        var result = await languageService.FindLanguageLevelsAsync();
        var isEqual = ServiceUtilities.CompareList(result.Value.ToArray(), orderedLanguagesLevel.ToArray());

        // Validations for tests
        Assert.True(result.IsSuccess && isEqual);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindLanguagesLevel_ReturnError()
    {
        // Declarations of variables
        var languagesLevel = new List<LanguageLevel>();
        var testError = "Error to find languages level";

        // Create the simulated service
        var languageService = ServiceUtilities.CreateLanguageService(out _, out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository);

        // Configurations for tests
        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await languageService.FindLanguageLevelsAsync();

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
