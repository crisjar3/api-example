using Ardalis.Result;
using Ardalis.Specification;
using NetForemost.Core.Dtos.Dashboard;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Interfaces.Dashboards;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.JobOffers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Dashboards;
public class DashboardService : IDashboardService
{
    private readonly int EVALUATIONMONTH = 6;

    private readonly IAsyncRepository<JobOffer> _jobOfferRepository;
    private readonly IAsyncRepository<Project> _projectRepository;
    private readonly IAsyncRepository<CompanyUser> _companyUserRepository;

    public DashboardService(IAsyncRepository<JobOffer> jobOfferRepository, IAsyncRepository<Project> projectRepository, IAsyncRepository<CompanyUser> companyUserRepository)
    {
        _jobOfferRepository = jobOfferRepository;
        _projectRepository = projectRepository;
        _companyUserRepository = companyUserRepository;
    }

    public async Task<Result<DashboardDto>> GetInformationForDashboardAsync()
    {
        try
        {
            var dashboard = new DashboardDto();

            var fromTo = DateTime.Today;
            var dateEvaluation = DateTime.Today.AddMonths(-EVALUATIONMONTH);

            //setting the value to go to the first day ago [EVALUATIONMONTH] month
            var upTo = dateEvaluation.AddDays(1 - dateEvaluation.Day);

            var jobOffers = await _jobOfferRepository.ListAsync(new GetJobOfferByDateSpecification(fromTo, upTo));
            var projects = await _projectRepository.ListAsync(new GetProjectByDateSpecification(fromTo, upTo));
            var teammates = await _companyUserRepository.ListAsync(new GetCompanyUserByDateSpecification(fromTo, upTo));

            // grouping the results by a dictionary by month
            var informationJobOffer =
                jobOffers
                .GroupBy(jobOffer => jobOffer.CreatedAt.ToString("yy/MM"))
                .Select(g => new { Date = g.Key, Value = g.Count() })
                .ToDictionary(g => g.Date, g => g.Value);

            var informationProjects =
                projects
                .GroupBy(jobOffer => jobOffer.CreatedAt.ToString("yy/MM"))
                .Select(g => new { Date = g.Key, Value = g.Count() })
                .ToDictionary(g => g.Date, g => g.Value);

            var informationTeammate =
                teammates
                .GroupBy(jobOffer => jobOffer.CreatedAt.ToString("yy/MM"))
                .Select(g => new { Date = g.Key, Value = g.Count() })
                .ToDictionary(g => g.Date, g => g.Value);


            //getting this month's records
            dashboard.CountLastMonthJobOffers =
                jobOffers
                .Where(jobOffer => jobOffer.CreatedAt.Year == DateTime.Today.Year && jobOffer.CreatedAt.Month == DateTime.Today.Month)
                .Count();

            dashboard.CountLastMonthProjects =
                projects
                .Where(project => project.CreatedAt.Year == DateTime.Today.Year && project.CreatedAt.Month == DateTime.Today.Month)
                .Count();

            dashboard.CountLastMonthTeammate =
                teammates
                .Where(companyUser => companyUser.CreatedAt.Year == DateTime.Today.Year && companyUser.CreatedAt.Month == DateTime.Today.Month)
                .Count();

            //adding the missing results
            dashboard.JobOffers = OrderResultsByMonths(informationJobOffer);
            dashboard.Projects = OrderResultsByMonths(informationProjects);
            dashboard.Teammates = OrderResultsByMonths(informationTeammate);

            return Result.Success(dashboard);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    private Dictionary<string, int> OrderResultsByMonths(Dictionary<string, int> dates)
    {
        var newInformation = new Dictionary<string, int>();

        for (int i = 0; i <= (EVALUATIONMONTH - 1); i++)
        {
            var date = DateTime.Now.AddMonths(-i);
            var key = date.ToString("yy/MM");

            var value = dates.ContainsKey(key) ? dates[key] : 0;
            newInformation.Add(key, value);
        }

        return newInformation;
    }
}
