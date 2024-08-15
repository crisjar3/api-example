using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class GetCompanyUserDetailsRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
