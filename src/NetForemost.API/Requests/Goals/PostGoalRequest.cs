using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Goals
{
    public class PostGoalRequest
    {

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime TargetEndDate { get; set; }

        public bool Approved { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double EstimatedHours { get; set; }

        [Required]
        public int StoryPointId { get; set; }

        public string JiraTicketId { get; set; }

        [Required]
        public int PriorityLevelId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string ScrumMasterId { get; set; }

        [Required]
        public int GoalStatusId { get; set; }
    }
}
