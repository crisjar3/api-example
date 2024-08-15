

namespace NetForemost.Core.Dtos.Goals
{
    public class GoalExtraMileDto
    {
        public int Id { get; set; }
        public DateTime ExtraMileTargetEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int GoalId { get; set; }
        public virtual GoalDto Goal { get; set; }
        public int? GoalStatusId { get; set; }
        public virtual GoalStatusDto? GoalStatus { get; set; }
    }
}
