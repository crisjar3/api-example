using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Goals
{
    public class PutConfirmExtraMileGoalRequest
    {

        [Required]
        public int ExtraMileGoalId { get; set; }

        [Required]
        public int GoalStatusId { get; set; }

    }
}
