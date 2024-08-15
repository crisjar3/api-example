using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Goals;

public class GetGoalStatusByCompanyRequest
{
    [Required]
    public int CompanyId { get; set; }
}
