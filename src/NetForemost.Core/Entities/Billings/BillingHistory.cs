using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Billings;

public class BillingHistory : BaseEntity
{
    public DateTime Date { get; set; }
    public int AccountsNumber { get; set; }
    public double SubscriptionPrice { get; set; }
    public double TotalBilling { get; set; }
    public int SubscriptionId { get; set; }
    public int SubscriptionTypeId { get; set; }

    public Subscription Subscription { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
}