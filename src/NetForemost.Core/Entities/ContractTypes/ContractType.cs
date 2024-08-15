using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.ContractTypes;
public class ContractType : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? CompanyId { get; set; }

    public virtual Company? Company { get; set; }
}

