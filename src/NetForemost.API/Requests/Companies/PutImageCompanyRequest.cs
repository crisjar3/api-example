using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class PutImageCompanyRequest
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string UserImageUrl { get; set; }
    }
}
