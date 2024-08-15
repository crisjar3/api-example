using Ardalis.Specification;
using NetForemost.Core.Entities.Languages;

namespace NetForemost.Core.Specifications.Languages
{
    public class FindLanguageByIdSpecification : Specification<Language>
    {
        public FindLanguageByIdSpecification(int idLanguage)
        {
            Query.Where(language => language.Id == idLanguage);

        }
    }
}
