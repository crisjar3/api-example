using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Tasks
{
    public class PutTaskRequest
    {
        [Required]
        public int Id { get; set; }

        public string? Description { get; set; }

        public DateTime? TargetEndDate { get; set; }
        public int? TypeId { get; set; }
        public int? GoalId { get; set; }

    }
}
