using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class GetCompanyRequest
    {
        [Required]
        public int CompanyId { get; set; }
    }
}
