using NetForemost.SharedKernel.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Projects;
public class GetProjectRequest : PaginationRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public int CompanyId { get; set; }
    public int ProjectId { get; set; }
    public string UserId { get; set; }
    public DateTime? DateStartFrom { get; set; }
    public DateTime? DateEndFrom { get; set; }
    public DateTime? DateStartTo { get; set; }
    public DateTime? DateEndTo { get; set; }
    public float BudgetRangeFrom { get; set; }
    public float BudgetRangeTo { get; set; }
    public string[] TechStack { get; set; }
}
