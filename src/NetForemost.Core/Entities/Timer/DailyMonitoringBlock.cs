using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Timer;

public class DailyMonitoringBlock : BaseEntity
{
    public double KeystrokesMin { get; set; }
    public double MouseMovementsMin { get; set; }
    public string UrlImage { get; set; }
    public int DailyTimeBlockId { get; set; }
    public virtual DailyTimeBlock DailyTimeBlock { get; set; }

    public void SetOwner(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }
}