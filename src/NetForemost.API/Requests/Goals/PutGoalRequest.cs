using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Goals;

public class PutGoalRequest
{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ScrumMasterId { get; set; }
    public int ProjectId { get; set; }
    public int StoryPointId { get; set; }
    public int PriorityLevelId { get; set; }
}
