using NetForemost.SharedKernel.Entities;
using System;

namespace NetForemost.API.Requests.Tasks
{
    public class GetTasksRequest : PaginationRequest
    {
        public string? Search { get; set; }
        public DateTime? TargetEndDateFrom { get; set; }
        public DateTime? TargetEndDateTo { get; set; }
        public int TypeId { get; set; }
        public int GoalId { get; set; }
        public int ProjectId { get; set; }
        public int CompanyId { get; set; }

    }
}
