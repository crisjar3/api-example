using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects
{
    public class PutProjectCompanyUserStatusListRequest
    {
        [Required]
        public int CompanyUserId { get; set; }
        [Required]
        public int[] ProjectListIds { get; set; }
    }
}
