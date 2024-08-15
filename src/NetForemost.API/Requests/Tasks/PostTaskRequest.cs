using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Tasks
{
    public class PostTaskRequest
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime TargetEndDate { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int TypeId { get; set; }
        public int? GoalId { get; set; }

        [Required]
        public string OwnerId { get; set; }
    }
}
