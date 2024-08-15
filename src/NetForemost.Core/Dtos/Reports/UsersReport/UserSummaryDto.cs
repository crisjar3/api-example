namespace NetForemost.Core.Dtos.Reports.UsersReport;

public class UserSummaryDto
{
    public string ImageUrl { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; }
    public string UserName { get; set; }
    public string TimeWorked { get; set; }
    public int GoalWorked { get; set; }
}
