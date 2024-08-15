using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Benefits;

public class GetBenefitsRequest
{
    [Required]
    public int CompanyId { get; set; }
}