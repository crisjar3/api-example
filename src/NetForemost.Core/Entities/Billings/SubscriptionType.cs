using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Billings;

public class SubscriptionType : BaseEntity
{
    public string Name { get; set; }
    public double Price { get; set; }
}