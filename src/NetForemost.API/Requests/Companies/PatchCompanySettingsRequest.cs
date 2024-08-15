using Microsoft.AspNetCore.JsonPatch.Operations;
using NetForemost.Core.Entities.Companies;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies
{
    public class PatchCompanySettingsRequest
    {
        [Required]
        public List<Operation<CompanySettings>> PatchCompanySettings { get; set; }
    }
}
