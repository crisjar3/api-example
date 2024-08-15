using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Benefits;

public class Benefit : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDefault { get; set; }
    public int? CompanyId { get; set; }

    public Company? Company { get; set; }
}