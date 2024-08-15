using Ardalis.Specification;
using NetForemost.Core.Entities.Languages;

namespace NetForemost.Core.Specifications.Languages
{
    public class LanguageCodeSpecification : Specification<Language>
    {
        public LanguageCodeSpecification(string languageCode)
        {
            Query.Where(la => la.Code == languageCode);
        }
    }
}
