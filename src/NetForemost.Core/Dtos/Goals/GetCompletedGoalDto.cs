namespace NetForemost.Core.Dtos.Goals;
public class GetCompletedGoalDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime TargetEndTime { get; set; }
}
