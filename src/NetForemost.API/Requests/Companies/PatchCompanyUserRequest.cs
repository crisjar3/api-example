using Microsoft.AspNetCore.JsonPatch.Operations;
using NetForemost.Core.Entities.Companies;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Companies;

public class PatchCompanyUserRequest
{
    [Required]
    public int CompanyUserId { get; set; }
    [Required]
    public List<Operation<CompanyUser>> PatchCompanyUser { get; set; }
}