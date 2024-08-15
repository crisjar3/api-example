namespace NetForemost.Core.Dtos.Goals;

public class GoalDataReportDto
{
    public int GoalId { get; set; }
    public string GoalName { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string PriorityLevel { get; set; }
    public string TimeWorked { get; set; }
    public string UserName { get; set; }
}
