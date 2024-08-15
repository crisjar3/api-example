namespace NetForemost.Core.Dtos.Reports.ProjectsReport;

public class ProjectExportDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public string TimeWorked { get; set; }
    public int GoalsWorked { get; set; }
    public int UsersWorked { get; set; }
}
