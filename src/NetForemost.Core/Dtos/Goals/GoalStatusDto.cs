namespace NetForemost.Core.Dtos.Goals;

public class GoalStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public GoalStatusCategoryDto? StatusCategory { get; set; }
}
