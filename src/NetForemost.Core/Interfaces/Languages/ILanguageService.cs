using Ardalis.Result;
using NetForemost.Core.Entities.Languages;

namespace NetForemost.Core.Interfaces.Languages;

public interface ILanguageService
{
    /// <summary>
    /// Find the list of all languages
    /// </summary>
    /// <returns></returns>
    Task<Result<List<Language>>> FindLanguagesAsync();

    /// <summary>
    /// Find the language level
    /// </summary>
    /// <returns></returns>
    Task<Result<List<LanguageLevel>>> FindLanguageLevelsAsync();
}