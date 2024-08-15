using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Industries;

public class Industry : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<Company> Companies { get; set; }
}