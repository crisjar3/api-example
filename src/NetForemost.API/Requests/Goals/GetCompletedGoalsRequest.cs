using NetForemost.SharedKernel.Entities;
using System;

namespace NetForemost.API.Requests.Goals;
public class GetCompletedGoalsRequest : PaginationRequest
{
    public DateTime From { get; set; } = DateTime.MinValue;
    public DateTime To { get; set; } = DateTime.MaxValue;
}