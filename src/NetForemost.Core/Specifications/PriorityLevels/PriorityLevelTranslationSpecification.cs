using Ardalis.Specification;
using NetForemost.Core.Entities.PriorityLevels;

namespace NetForemost.Core.Specifications.PriorityLevels
{
    public class PriorityLevelTranslationSpecification : Specification<PriorityLevelTranslation, PriorityLevel>
    {
        public PriorityLevelTranslationSpecification(int languageId)
        {

            Query.Include(x => x.PriorityLevel);
            Query.Where(x => x.LanguageId == languageId);

            Query.Select(priorityLevelTranslation => new PriorityLevel
            {
                Id = priorityLevelTranslation.PriorityLevelId,
                Level = priorityLevelTranslation.Level,
                Description = priorityLevelTranslation.Description,
                HexColorCode = priorityLevelTranslation.PriorityLevel.HexColorCode
            });
        }
    }
}
