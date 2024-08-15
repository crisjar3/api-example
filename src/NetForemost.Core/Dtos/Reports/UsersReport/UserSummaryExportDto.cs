namespace NetForemost.Core.Dtos.Reports.UsersReport;

public class UserSummaryExportDto
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string TimeWorked { get; set; }
    public int GoalWorked { get; set; }
}
