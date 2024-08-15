namespace NetForemost.Core.Dtos.Goals
{
    public class GoalExtraMileResumeDto
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public virtual GoalResumeDto Goal { get; set; }
        public DateTime ExtraMileTargetEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public virtual GoalStatusDto GoalStatus { get; set; }
    }
}