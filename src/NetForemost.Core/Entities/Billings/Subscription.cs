using NetForemost.Core.Entities.Companies;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Billings;

public class Subscription : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CompanyId { get; set; }
    public int SubscriptionTypeId { get; set; }

    public Company Company { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
}