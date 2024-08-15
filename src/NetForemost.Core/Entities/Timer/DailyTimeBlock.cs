using NetForemost.Core.Entities.Projects;
using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetForemost.Core.Entities.Timer;

[Table("daily_time_block")]
public class DailyTimeBlock : BaseEntity
{
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public int TaskId { get; set; }
    public int? DeviceId { get; set; }
    public virtual Device? Device { get; set; }
    public virtual Tasks.Task Task { get; set; }
    public virtual ICollection<DailyMonitoringBlock> Monitorings { get; set; } = new List<DailyMonitoringBlock>();
    public int? OwnerId { get; set; }
    public virtual ProjectCompanyUser? Owner { get; set; }

    public void SetOwner(string userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public void ConvertDatesToTimezone(int offsetHours)
    {
        // Add the offset to TimeStart and TimeEnd
        TimeStart = TimeStart.AddHours(-offsetHours);
        TimeEnd = TimeEnd.AddHours(-offsetHours);
    }

    public void AddMonitoringBlock(DailyMonitoringBlock blockMonitoring)
    {
        //Update strokes by Min
        //DailyEntry.SetAverageStrokesAndMovement(blockMonitoring);

        //add monitoring
        Monitorings.Add(blockMonitoring);
        blockMonitoring.DailyTimeBlock = this;
    }

    public bool IsTimeEndGreaterThanTimeStart()
    {
        return TimeEnd > TimeStart;
    }
}
