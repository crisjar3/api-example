namespace NetForemost.Core.Dtos.Timer;

public class GetAllTimeBlocksByUserPerDayDto
{
    public DateTime Date { get; set; }
    public int GoalsWorked { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string TotalHoursWorked { get; set; }
    public List<GetDailyTimeBlockDto> TimeBlocks { get; set; }

    public GetAllTimeBlocksByUserPerDayDto()
    {
        GoalsWorked = 0;
        StartTime = new("00:00:00");
        EndTime = new("00:00:00");
        TotalHoursWorked = "0";
        TimeBlocks = new List<GetDailyTimeBlockDto>();
    }
}

public class GetDailyTimeBlockDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Task { get; set; }
    public string Project { get; set; }
    public double HoursWorked => (EndTime - StartTime).TotalHours;
    public string HoursWorkedInLocalFormat => (EndTime - StartTime).ToString(@"hh\:mm\:ss");
}