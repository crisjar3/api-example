using NetForemost.Core.Dtos.Cities;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Dtos.Countries;
using NetForemost.Core.Dtos.Projects;

namespace NetForemost.Core.Dtos.JobOffers;

public class JobOfferDto
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateExpiration { get; set; }

    public CityDto? City { get; set; }
    public CountryDto? Country { get; set; }
    public CompanyDto Company { get; set; }
    public ProjectDto? Project { get; set; }

    public List<JobOfferBenefitDto> Benefits { get; set; }
    public List<JobOfferTalentDto> Talents { get; set; }
}