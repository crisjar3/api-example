using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Billings;

public class SubscriptionTypeFeature : BaseEntity
{
    public int SubscriptionTypeId { get; set; }
    public int FeatureId { get; set; }

    public virtual SubscriptionType SubscriptionType { get; set; }
    public virtual Feature Feature { get; set; }
}