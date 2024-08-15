using Moq;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Entities.Skills;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.JobOffers.Services;

public class CreateJobOfferTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Create a Job Offer
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenCreateJobOfferAsyncIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,

        };
        var userId = "user_id";

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the City does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return City Not Found </returns>
    [Fact]
    public async Task WhenCityNotFound_ReturnCityNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CityNotFound);
    }

    /// <summary>
    /// It checks if the Country does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Country Not Found </returns>
    [Fact]
    public async Task WhenCountryNotFound_ReturnCountryNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Country());

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CountryNotFound);
    }

    /// <summary>
    /// It checks if the Project does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Project Not Found </returns>
    [Fact]
    public async Task WhenProjectNotFound_ReturnProjectNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Project());

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ProjectNotFound);
    }

    /// <summary>
    /// It checks if the Company does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Company Not Found </returns>
    [Fact]
    public async Task WhenCompanyNotFound_ReturnCompanyNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Company());

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// It checks if the Benefit Id does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Benefit Id Not Found </returns>
    [Fact]
    public async Task WhenBenefitNotFound_ReturnBenefitIdNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.BenefitIdNotFound.Replace("[id]", benefit.BenefitId.ToString()));
    }

    /// <summary>
    /// It checks if the Job Roles does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Job Role Id Not Found </returns>
    [Fact]
    public async Task WhenJobRoleNotFound_ReturnJobRoleNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new JobRole());

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.JobRoleIdNotFound.Replace("[id]", jobOfferTalent.JobRoleId.ToString()));
    }

    /// <summary>
    /// It checks if the Seniority does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Seniority Id Not Found </returns>
    [Fact]
    public async Task WhenSeniorityNotFound_ReturnSeniorityIdNotFoud()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Seniority());

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.SeniorityIdNotFound.Replace("[id]", jobOfferTalent.SeniorityId.ToString()));
    }

    /// <summary>
    /// It checks if the Policy does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Policy Not Found </returns>
    [Fact]
    public async Task WhenPolicyNotFound_ReturnPolicyNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
           It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Policy());

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.PolicyNotFound.Replace("[id]", jobOfferTalent.PolicyId.ToString()));
    }

    /// <summary>
    /// It checks if the Contract type does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Contract Type Not Found </returns>
    [Fact]
    public async Task WhenContractTypeNotFound_ReturnContractTypeNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
           It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ContractTypeNotFound.Replace("[id]", jobOfferTalent.ContractTypeId.ToString()));
    }

    /// <summary>
    /// It checks if the Language does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Language Not Found </returns>
    [Fact]
    public async Task WhenLanguageNotFound_ReturnLanguageNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
           It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Language());

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.LanguageNotFound.Replace("[id]", jobOfferTalent.LanguageId.ToString()));
    }

    /// <summary>
    /// It checks if the Language Level does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Language Level Not Found </returns>
    [Fact]
    public async Task WhenLanguageLevelNotFound_ReturnLanguageLevel()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
           It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            idNotExist,
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new LanguageLevel());

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.LanguageLevelNotFound.Replace("[id]", jobOfferTalent.LanguageLevelId.ToString()));
    }

    /// <summary>
    /// It checks if the Skill Id does'nt exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Skill Id Not Found </returns>
    [Fact]
    public async Task WhenSkillNotFound_ReturnSkillIdNotFound()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalenSkill = new JobOfferTalentSkill();
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { jobOfferTalenSkill }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";
        var idNotExist = 2;

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(city);

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
           It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
             It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           idNotExist,
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(new Skill());

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.SkillIdNotFound.Replace("[id]", jobOfferTalenSkill.SkillId.ToString()));
    }

    /// <summary>
    /// It checks if the number if vacancies is invalid and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return Error, The number of vacancies must be greater than zero </returns>
    [Fact]
    public async Task WhenNumberOfVacanciesIsInvalid_ReturnTheNumberOfVacanciesMustBeGreaterThanZero()
    {
        //Delcarations of variables
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 0 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,

        };
        var userId = "user_id";

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), "The number of vacancies must be greater than zero");
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurWithCreateJobOffer_ReturnError()
    {
        //Delcarations of variables
        var testError = "Error to create job offer";
        var city = new City();
        var country = new Country();
        var company = new Company();
        var benefit = new JobOfferBenefit();
        var benefits = new List<JobOfferBenefit>() { benefit };
        var jobOfferTalent = new JobOfferTalent() { JobOfferTalentSkills = new List<JobOfferTalentSkill>() { new JobOfferTalentSkill() }, Vacancie = 1 };
        var jobOffersTalents = new List<JobOfferTalent>() { jobOfferTalent };
        var jobRole = new JobRole();
        var seniority = new Seniority();
        var skill = new Skill();
        var policy = new Policy();
        var contractType = new ContractType();
        var language = new Language();
        var languageLevel = new LanguageLevel();
        var project = new Project();

        var jobOffer = new JobOffer()
        {
            Id = 1,
            City = city,
            CityId = city.Id,
            Country = country,
            CountryId = country.Id,
            Company = company,
            Benefits = benefits,
            Project = project,
            ProjectId = project.Id,
            Talents = jobOffersTalents,
            IsActive = true,
            DateExpiration = DateTime.UtcNow,
        };
        var userId = "user_id";

        //Create the simulated service
        var teamService = ServiceUtilities.CreateJobOfferService(
           out Mock<IAsyncRepository<City>> cityRepository,
           out Mock<IAsyncRepository<Country>> countryRepository,
           out Mock<IAsyncRepository<Company>> companyRepository,
           out Mock<IAsyncRepository<Benefit>> benefitRepository,
           out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
           out Mock<IAsyncRepository<Seniority>> seniorityRepository,
           out Mock<IAsyncRepository<Skill>> skillRepository,
           out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
           out Mock<IAsyncRepository<Policy>> policyRepository,
           out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
           out Mock<IAsyncRepository<Language>> languageRepository,
           out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
           out Mock<IAsyncRepository<Project>> projectRepository
            );

        //Configurations for tests
        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        countryRepository.Setup(countryRepository => countryRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(country);

        projectRepository.Setup(projectRepository => projectRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(project);

        companyRepository.Setup(companyRepository => companyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(company);

        benefitRepository.Setup(benefitRepository => benefitRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Benefit());

        jobRoleRepository.Setup(jobRoleRepository => jobRoleRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobRole);

        seniorityRepository.Setup(seniorityRepository => seniorityRepository.GetByIdAsync(
           It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(seniority);

        policyRepository.Setup(policyRepository => policyRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(policy);

        contractTypeRepository.Setup(contractTypeRepository => contractTypeRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ContractType());

        languageRepository.Setup(languageRepository => languageRepository.GetByIdAsync(
             It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(language);

        languageLevelRepository.Setup(languageLevelRepository => languageLevelRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(languageLevel);

        jobOfferRepository.Setup(jobOfferRepository => jobOfferRepository.GetByIdAsync(
            It.IsAny<int?>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(jobOffer);

        skillRepository.Setup(skillRepository => skillRepository.GetByIdAsync(
           It.IsAny<int?>(),
           It.IsAny<CancellationToken>()
           )).ReturnsAsync(skill);

        var result = await teamService.CreateJobOfferAsync(jobOffer, userId);

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
