namespace NetForemost.Core.Dtos.Dashboard;
public class DashboardDto
{
    public int CountLastMonthJobOffers { get; set; }

    public Dictionary<string, int>? JobOffers { get; set; }

    public int CountLastMonthProjects { get; set; }

    public Dictionary<string, int>? Projects { get; set; }

    public int CountLastMonthTeammate { get; set; }

    public Dictionary<string, int>? Teammates { get; set; }
}

