

namespace NetForemost.Core.Dtos.StoryPoints
{
    public class StoryPointDto
    {
        public int Id { get; set; }
        public string? KnowledgeLevel { get; set; }
        public string? Dependencies { get; set; }
        public string? WorkEffort { get; set; }
        public int? Points { get; set; }
    }
}
