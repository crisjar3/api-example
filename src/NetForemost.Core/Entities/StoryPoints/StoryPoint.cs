using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.StoryPoints
{
    public class StoryPoint : BaseEntity
    {
        public string? KnowledgeLevel { get; set; }
        public string? Dependencies { get; set; }
        public string? WorkEffort { get; set; }
        public int? Points { get; set; }

    }
}
