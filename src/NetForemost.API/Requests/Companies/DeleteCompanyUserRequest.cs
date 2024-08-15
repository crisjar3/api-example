using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class DeleteCompanyUserRequest
    {
        [Required]
        public int CompanyuserId { get; set; }
    }
}
