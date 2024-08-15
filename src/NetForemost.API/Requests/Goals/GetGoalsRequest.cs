using NetForemost.SharedKernel.Entities;
using System;

namespace NetForemost.API.Requests.Goals
{
    public class GetGoalsRequest : PaginationRequest
    {
        public string Description { get; set; }
        public double EstimatedHours { get; set; }
        public int ProjectId { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? ActualEndDateFrom { get; set; }
        public DateTime? ActualEndDateTo { get; set; }
        public DateTime CreationDateTo { get; set; } = DateTime.MinValue;
        public DateTime CreationDateFrom { get; set; } = DateTime.MinValue;
        public int StoryPoints { get; set; }
        public string ScrumMasterId { get; set; }
        public string JiraTicketId { get; set; }
        public string PriorityLevel { get; set; }
        public int GoalStatusId { get; set; }
        public int CompanyId { get; set; }
    }
}
