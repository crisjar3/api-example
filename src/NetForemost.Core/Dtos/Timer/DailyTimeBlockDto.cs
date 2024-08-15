namespace NetForemost.Core.Dtos.Timer;
public class DailyTimeBlockDto
{
    public int Id { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public int TaskId { get; set; }
    public int? DeviceId { get; set; }
    public int DailyEntryId { get; set; }
}
