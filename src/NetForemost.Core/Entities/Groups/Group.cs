using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Groups;

public class Group : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CompanyId { get; set; }

    public Company Company { get; set; }
}