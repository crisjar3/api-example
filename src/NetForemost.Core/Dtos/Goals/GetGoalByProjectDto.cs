namespace NetForemost.Core.Dtos.Goals;

public class GetGoalByProjectDto
{
    public int GoalId { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsExtraMile { get; set; }
    public string Description { get; set; }
    public string TimeWorked { get; set; }
    public string EstimatedTime { get; set; }
    public int Points { get; set; }
    public string Priority { get; set; }
    public string Status { get; set; }
}
