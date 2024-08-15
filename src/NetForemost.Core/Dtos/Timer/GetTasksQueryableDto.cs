namespace NetForemost.Core.Dtos.Timer;
public class GetTasksQueryableDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public GetTypeTaskDto Type { get; set; }
    public int? GoalId { get; set; }
    public int TimeSpentInSecond { get; set; }
}

public class GetTypeTaskDto
{
    public int Id { get; set; }
    public string Description { get; set; }
}