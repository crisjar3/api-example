using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Goals
{
    public class PutConfirmGoalRequest
    {

        [Required]
        public int GoalId { get; set; }

        [Required]
        public int GoalStatusId { get; set; }

    }
}
