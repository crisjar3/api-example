using NetForemost.SharedKernel.Entities;
using System;

namespace NetForemost.API.Requests.Goals;

public class GetGoalsExtraMileRequest : PaginationRequest
{
    public int goalId { get; set; }
    public string Description { get; set; }
    public int ProjectId { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public DateTime? ActualEndDateFrom { get; set; }
    public DateTime? ActualEndDateTo { get; set; }
    public int StoryPoints { get; set; }
    public string ScrumMasterId { get; set; }
    public string JiraTicketId { get; set; }
    public string PriorityLevel { get; set; }
    public int GoalStatusId { get; set; }
}
