using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies;

public class PostCompanyUserArchiveRequest
{
    [Required]
    public int CompanyUserId { get; set; }
}