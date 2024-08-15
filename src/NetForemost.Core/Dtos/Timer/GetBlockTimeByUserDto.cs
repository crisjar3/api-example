namespace NetForemost.Core.Dtos.Timer;

public class GetBlockTimeByUserDto
{
    public int Id { get; set; }
    public string Project { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public string TaskDescription { get; set; }
    public string Goal { get; set; }
    public string TotalWorkedInHours { get; set; }
    public string TaskType { get; set; }

    public static GetBlockTimeByUserDto BlockNonWorked(DateTime dateStart, DateTime dateEnd)
    {
        return new GetBlockTimeByUserDto()
        {
            Id = 0,
            TimeStart = dateStart,
            TimeEnd = dateEnd,
            TaskDescription = "Not Working",
            Goal = "Not Working",
            TotalWorkedInHours = FormatTimeDifference(dateStart, dateEnd),
            TaskType = "Not Working"
        };
    }

    public static string FormatTimeDifference(DateTime dateStart, DateTime dateEnd)
    {
        var timeDifference = dateEnd - dateStart;

        if (timeDifference.TotalMinutes < 60)
        {
            return $"{(int)timeDifference.TotalMinutes}m";
        }
        else
        {
            return $"{(int)timeDifference.TotalHours}h {(int)timeDifference.Minutes}m";
        }
    }

}
