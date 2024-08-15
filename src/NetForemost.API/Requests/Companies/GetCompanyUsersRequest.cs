using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class GetCompanyUsersRequest : PaginationRequest
    {
        [Required]
        public int CompanyId { get; set; }

    }
}
