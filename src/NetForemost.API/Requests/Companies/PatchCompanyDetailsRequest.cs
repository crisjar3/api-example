using Microsoft.AspNetCore.JsonPatch.Operations;
using NetForemost.Core.Entities.Companies;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class PatchCompanyDetailsRequest
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public List<Operation<Company>> PatchCompany { get; set; }
    }
}
