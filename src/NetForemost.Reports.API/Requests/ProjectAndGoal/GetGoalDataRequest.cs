using System.ComponentModel.DataAnnotations;

namespace NetForemost.Reports.API.Requests.ProjectAndGoal
{
    public class GetGoalDataRequest
    {
        [Required]
        public int GoalId { get; set; }
        [Required]
        public double TimeZone { get; set; }
    }
}
