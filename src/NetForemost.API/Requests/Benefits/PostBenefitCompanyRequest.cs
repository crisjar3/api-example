using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Benefits;

public class PostBenefitCompanyRequest
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; } = "";
    [Required]
    public int CompanyId { get; set; }
}
