using NetForemost.API.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Goals
{
    public class PostExtraMileGoalRequest
    {

        [Required]
        [DateGreaterThanToday]
        public DateTime ExtraMileTargetEndDate { get; set; }

        [Required]
        public int GoalId { get; set; }
        [Required]
        public int GoalStatusId { get; set; }

    }
}
