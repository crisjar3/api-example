using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Goals;

public class PutGoalStatusOfGoalRequest
{
    [Required]
    [FromRoute(Name = "goalId")]
    public int GoalId { get; set; }

    [FromBody]
    public PutGoalStatusBody Body { get; set; }
}

public class PutGoalStatusBody
{
    [Required]
    public int GoalStatusId { get; set; }
}
