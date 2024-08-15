using Moq;
using NetForemost.Core.Dtos.Dashboard;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Specifications.CompanyUsers;
using NetForemost.Core.Specifications.JobOffers;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Dashboards.Services;

public class GetInformationForDashboardAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Get Information for Dashboard
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenGetInformationForTheDashboardAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var jobOffers = new List<JobOffer>();
        var projects = new List<Project>();
        var companyUsers = new List<CompanyUser>();

        // Create the simulated service
        var dashboardService = ServiceUtilities.CreateDashboardService(
            out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository
            );

        // Configurations for tests
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.ListAsync(
            It.IsAny<GetJobOfferByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffers);

        projectRepository.Setup(projectRepository => projectRepository.ListAsync(
            It.IsAny<GetProjectByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(projects);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<GetCompanyUserByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companyUsers);

        var result = await dashboardService.GetInformationForDashboardAsync();

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Check if the response information from the DashboardDto is valid
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenGetDtoInformationIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var dashboard = new DashboardDto();
        var date = DateTime.UtcNow;

        var jobOffers = new List<JobOffer>()
        {
            new JobOffer(){Id = 1, CreatedAt = date},
            new JobOffer(){Id = 2, CreatedAt = date.AddSeconds(1)},
            new JobOffer(){Id = 3, CreatedAt = date.AddSeconds(2)},
            new JobOffer(){Id = 4, CreatedAt = date.AddSeconds(3)},
        };

        var projects = new List<Project>()
        {
            new Project(){Id = 1, CreatedAt = date},
            new Project(){Id = 2, CreatedAt = date.AddSeconds(1)},
            new Project(){Id = 3, CreatedAt = date.AddSeconds(1)},
            new Project(){Id = 4, CreatedAt = date.AddSeconds(1)},

        };

        var companyUsers = new List<CompanyUser>()
        {
            new CompanyUser(){Id = 1, CreatedAt = date},
            new CompanyUser(){Id = 2, CreatedAt = date.AddSeconds(1)},
            new CompanyUser(){Id = 3, CreatedAt = date.AddSeconds(2)},
            new CompanyUser(){Id = 4, CreatedAt = date.AddSeconds(3)},
        };

        var informationJobOffer = new Dictionary<string, int>
        {
            { $"{date.ToString("yy/MM")}", 4 },
            { $"{date.AddMonths(-1).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-2).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-3).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-4).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-5).ToString("yy/MM")}", 0 },
        };

        var informationProjects = new Dictionary<string, int>
        {
            { $"{date.ToString("yy/MM")}", 4 },
            { $"{date.AddMonths(-1).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-2).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-3).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-4).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-5).ToString("yy/MM")}", 0 },
        };

        var informationTeammate = new Dictionary<string, int>
        {
            { $"{date.ToString("yy/MM")}", 4 },
            { $"{date.AddMonths(-1).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-2).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-3).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-4).ToString("yy/MM")}", 0 },
            { $"{date.AddMonths(-5).ToString("yy/MM")}", 0 },
        };

        dashboard.CountLastMonthJobOffers =
            jobOffers
            .Where(jobOffer => jobOffer.CreatedAt.Year == DateTime.Today.Year && jobOffer.CreatedAt.Month == DateTime.Today.Month)
            .Count();

        dashboard.CountLastMonthProjects =
            projects
            .Where(project => project.CreatedAt.Year == DateTime.Today.Year && project.CreatedAt.Month == DateTime.Today.Month)
            .Count();

        dashboard.CountLastMonthTeammate =
            companyUsers
            .Where(companyUser => companyUser.CreatedAt.Year == DateTime.Today.Year && companyUser.CreatedAt.Month == DateTime.Today.Month)
            .Count();

        // Create the simulated service
        var dashboardService = ServiceUtilities.CreateDashboardService(
            out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository
            );

        // Configurations for tests
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.ListAsync(
            It.IsAny<GetJobOfferByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffers);

        projectRepository.Setup(projectRepository => projectRepository.ListAsync(
            It.IsAny<GetProjectByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(projects);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<GetCompanyUserByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companyUsers);

        dashboard.JobOffers = informationJobOffer;
        dashboard.Projects = informationProjects;
        dashboard.Teammates = informationTeammate;

        var result = await dashboardService.GetInformationForDashboardAsync();

        var areEqualJobOffers = ServiceUtilities.CompareDictionary(result.Value.JobOffers, dashboard.JobOffers);
        var areEqualProjects = ServiceUtilities.CompareDictionary(result.Value.Projects, dashboard.Projects);
        var areEqualTeammates = ServiceUtilities.CompareDictionary(result.Value.Teammates, dashboard.Teammates);

        var areEqualInformation = dashboard.CountLastMonthJobOffers == result.Value.CountLastMonthJobOffers &&
                                  dashboard.CountLastMonthProjects == result.Value.CountLastMonthProjects &&
                                  dashboard.CountLastMonthTeammate == result.Value.CountLastMonthTeammate;

        // Validations for tests
        Assert.True(result.IsSuccess && areEqualJobOffers && areEqualProjects && areEqualTeammates && areEqualInformation);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccur_ReturnError()
    {
        // Declarations of variables
        var jobOffers = new List<JobOffer>();
        var projects = new List<Project>();
        var companyUsers = new List<CompanyUser>();
        var testError = "Error to Get information for the Dashboard";

        // Create the simulated service
        var dashboardService = ServiceUtilities.CreateDashboardService(
            out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
            out Mock<IAsyncRepository<Project>> projectRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository
            );

        // Configurations for tests
        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.ListAsync(
            It.IsAny<GetJobOfferByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        projectRepository.Setup(projectRepository => projectRepository.ListAsync(
            It.IsAny<GetProjectByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(projects);

        companyUserRepository.Setup(companyUserRepository => companyUserRepository.ListAsync(
            It.IsAny<GetCompanyUserByDateSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(companyUsers);

        var result = await dashboardService.GetInformationForDashboardAsync();

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
