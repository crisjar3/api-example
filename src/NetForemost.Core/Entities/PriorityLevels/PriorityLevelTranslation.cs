using NetForemost.Core.Entities.Languages;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.PriorityLevels
{
    public class PriorityLevelTranslation : BaseEntity
    {
        public string Level { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public int PriorityLevelId { get; set; }

        public virtual Language Language { get; set; }
        public virtual PriorityLevel PriorityLevel { get; set; }
    }
}
