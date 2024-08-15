using Ardalis.Result;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Interfaces.Languages;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Languages;

public class LanguageService : ILanguageService
{
    private readonly IAsyncRepository<Language> _languageRepository;
    private readonly IAsyncRepository<LanguageLevel> _languageLevelRepository;

    public LanguageService(IAsyncRepository<Language> languageRepository, IAsyncRepository<LanguageLevel> languageLevelRepository)
    {
        _languageRepository = languageRepository;
        _languageLevelRepository = languageLevelRepository;
    }

    public async Task<Result<List<Language>>> FindLanguagesAsync()
    {
        try
        {
            var languages = await _languageRepository.ListAsync();
            languages = languages.Where(language => language.IsActive).OrderBy(language => language.Name).ToList();

            return Result<List<Language>>.Success(languages);
        }
        catch (Exception ex)
        {
            return Result<List<Language>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<List<LanguageLevel>>> FindLanguageLevelsAsync()
    {
        try
        {
            var languageLevels = await _languageLevelRepository.ListAsync();
            languageLevels = languageLevels.OrderBy(languageLevel => languageLevel.Name).ToList();

            return Result<List<LanguageLevel>>.Success(languageLevels);
        }
        catch (Exception ex)
        {
            return Result<List<LanguageLevel>>.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}