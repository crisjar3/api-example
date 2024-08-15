using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Billings;

public class Feature : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}