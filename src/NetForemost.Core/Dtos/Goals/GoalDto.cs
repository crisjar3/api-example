using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.PriorityLevels;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Dtos.StoryPoints;

namespace NetForemost.Core.Dtos.Goals
{
    public class GoalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PriorityLevelDto PriorityLevel { get; set; }
        public StoryPointDto StoryPoint { get; set; }
        public string? JiraTicketId { get; set; }
        public ProjectDto? Project { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetEndDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public double EstimatedHours { get; set; }
        public bool Approved { get; set; }
        public bool HasExtraMileGoal { get; set; }
        public string ScrumMasterId { get; set; }
        public UserDto ScrumMaster { get; set; }
        public int? GoalStatusId { get; set; }
        public virtual GoalStatusDto? GoalStatus { get; set; }
    }
}
