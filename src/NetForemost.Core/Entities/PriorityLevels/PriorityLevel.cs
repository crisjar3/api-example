using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.PriorityLevels
{
    public class PriorityLevel : BaseEntity
    {
        public string? Level { get; set; }
        public string? Description { get; set; }
        public string? HexColorCode { get; set; }
    }
}
