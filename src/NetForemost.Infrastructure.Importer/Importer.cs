using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetForemost.Core.Entities.AppClients;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Entities.Skills;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Infrastructure.Importer.Entities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Infrastructure.Importer;

/// <summary>
/// Seed values class needed by the NetForemost Core
/// </summary>
public class Importer
{
    //Repositories
    private readonly IAsyncRepository<City> _cityRepository;
    private readonly IAsyncRepository<Country> _countryRepository;
    private readonly IAsyncRepository<Core.Entities.TimeZones.TimeZone> _timeZoneRepository;
    private readonly IAsyncRepository<Language> _languageRepository;
    private readonly RoleManager<Role> _roleManager;
    private readonly IAsyncRepository<RoleTranslation> _roleTranslationRepository;
    private readonly IAsyncRepository<JobRole> _jobRoleRepository;
    private readonly IAsyncRepository<JobRoleTranslation> _jobRoleTranslationRepository;
    private readonly IAsyncRepository<JobRoleCategory> _jobRoleCategoryRepository;
    private readonly IAsyncRepository<JobRoleCategoryTranslation> _jobRoleCategoryTranslationRepository;
    private readonly IAsyncRepository<Benefit> _benefitRepository;
    private readonly IAsyncRepository<Skill> _skillRepository;
    private readonly IAsyncRepository<Seniority> _seniorityRepository;
    private readonly IAsyncRepository<AppClient> _appClientRepository;
    private readonly IAsyncRepository<JobRoleCategorySkill> _jobRoleCategorySkillRepository;
    private readonly IAsyncRepository<Industry> _industryRepository;
    private readonly IAsyncRepository<LanguageLevel> _languageLevelRepository;
    private readonly IAsyncRepository<Policy> _policyRepository;
    private readonly IAsyncRepository<ContractType> _contractTypeRepository;
    private readonly IAsyncRepository<StoryPoint> _storyPointRepository;
    private readonly IAsyncRepository<PriorityLevel> _priorityLevelRepository;
    private readonly IAsyncRepository<PriorityLevelTranslation> _priorityLevelTranslationRepository;
    private readonly IAsyncRepository<Company> _companyRepository;
    private readonly IAsyncRepository<GoalStatusCategory> _goalStatusCategoryRepository;
    private readonly IAsyncRepository<GoalStatus> _goalStatusRepository;

    //Reference global variables for seed value creations and updates
    private readonly DateTime _currentDateUtcZero = DateTime.UtcNow;
    private readonly string _defaultUserId = "SYSTEM";

    //General
    private readonly ILogger<Importer> _logger;
    private readonly IMapper _mapper;

    public Importer(
        ILogger<Importer> logger,
        IMapper mapper,
        IAsyncRepository<Country> countryRepository,
        IAsyncRepository<City> cityRepository,
        IAsyncRepository<Core.Entities.TimeZones.TimeZone> timeZoneRepository,
        IAsyncRepository<Language> languageRepository,
        RoleManager<Role> roleManager,
        IAsyncRepository<RoleTranslation> roleTranslationRepository,
        IAsyncRepository<JobRole> jobRoleRepository,
        IAsyncRepository<JobRoleTranslation> jobRoleTranslationRepository,
        IAsyncRepository<JobRoleCategory> jobRoleCategoryRepository,
        IAsyncRepository<JobRoleCategoryTranslation> jobRoleCategoryTranslationRepository,
        IAsyncRepository<Benefit> benefitRepository,
        IAsyncRepository<Skill> skillRepository,
        IAsyncRepository<Seniority> seniorityRepository,
        IAsyncRepository<AppClient> appClientRepository,
        IAsyncRepository<JobRoleCategorySkill> jobRoleCategorySkillRepository,
        IAsyncRepository<Industry> industryRepository,
        IAsyncRepository<LanguageLevel> languageLevelRepository,
        IAsyncRepository<ContractType> contractTypeRepository,
        IAsyncRepository<Policy> policyRepository,
        IAsyncRepository<StoryPoint> storyPointRepository,
        IAsyncRepository<PriorityLevel> priorityLevelRepository,
        IAsyncRepository<PriorityLevelTranslation> priorityLevelTranslationRepository,
        IAsyncRepository<Company> companyRepository,
        IAsyncRepository<GoalStatusCategory> goalStatusCategoryRepository,
        IAsyncRepository<GoalStatus> goalStatusRepository
        )
    {
        _logger = logger;
        _mapper = mapper;
        _countryRepository = countryRepository;
        _cityRepository = cityRepository;
        _timeZoneRepository = timeZoneRepository;
        _languageRepository = languageRepository;
        _roleManager = roleManager;
        _roleTranslationRepository = roleTranslationRepository;
        _jobRoleRepository = jobRoleRepository;
        _jobRoleTranslationRepository = jobRoleTranslationRepository;
        _jobRoleCategoryRepository = jobRoleCategoryRepository;
        _jobRoleCategoryTranslationRepository = jobRoleCategoryTranslationRepository;
        _benefitRepository = benefitRepository;
        _skillRepository = skillRepository;
        _seniorityRepository = seniorityRepository;
        _appClientRepository = appClientRepository;
        _jobRoleCategorySkillRepository = jobRoleCategorySkillRepository;
        _industryRepository = industryRepository;
        _languageLevelRepository = languageLevelRepository;
        _contractTypeRepository = contractTypeRepository;
        _policyRepository = policyRepository;
        _storyPointRepository = storyPointRepository;
        _priorityLevelRepository = priorityLevelRepository;
        _priorityLevelTranslationRepository = priorityLevelTranslationRepository;
        _companyRepository = companyRepository;
        _goalStatusCategoryRepository = goalStatusCategoryRepository;
        _goalStatusRepository = goalStatusRepository;
    }

    public async Task RunAsync()
    {
        //Execute First
        await CreateLanguages("./Resources/languages.csv");
        await CreateAuthorizationRoles("./Resources/authorization_roles.csv");
        await CreateCountries("./Resources/countries.csv");
        await CreateTimeZones("./Resources/time_zones.csv");
        await CreateJobRoleCategories("./Resources/job_role_categories.csv");
        await CreateAppClients("./Resources/app_clients.csv");
        await CreateIndustries("./Resources/industries.csv");
        await CreateGoalStatusCategory("./Resources/goal_status_category.csv");
        await CreateGoalStatus("./Resources/goal_status.csv");


        //Execute Second
        await CreateCities("./Resources/cities.csv");
        await CreateJobRoles("./Resources/job_roles.csv");
        await CreateBenefits("./Resources/benefits.csv");
        await CreateSkills("./Resources/skills.csv");
        await CreateJobRoleCategorySkill("./Resources/job_role_category_skills.csv");
        await CreateSeniorities("./Resources/seniorities.csv");
        await CreatePolicies("./Resources/policies.csv");
        await CreateContractType("./Resources/contract_types.csv");
        await CreateStoryPoints("./Resources/storypoints.csv");
        await CreatePriorityLevels("./Resources/prioritylevels.csv");

        //Execute Last | Multilenguages 
        await CreateAuthorizationRolesTranslations("./Resources/authorization_roles_translations.csv");
        await CreatePriorityLevelTranslations("./Resources/priority_level_translation.csv");

    }

    #region Execute First
    private async Task CreateLanguages(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Language)}");

            var languagesCSV = CsvImporter<Language>.FromCsvPath(path);
            var languagesBD = await _languageRepository.ListAsync();

            foreach (var newLanguage in languagesCSV)
            {
                var existingLanguage = languagesBD.FirstOrDefault(languageBD => languageBD.Id == newLanguage.Id);

                if (existingLanguage != null)
                {
                    if (existingLanguage.Code != newLanguage.Code || existingLanguage.Name != newLanguage.Name ||
                        existingLanguage.Description != newLanguage.Description || existingLanguage.IsActive != newLanguage.IsActive)
                    {
                        existingLanguage.Code = newLanguage.Code;
                        existingLanguage.Name = newLanguage.Name;
                        existingLanguage.Description = newLanguage.Description;
                        existingLanguage.IsActive = newLanguage.IsActive;

                        existingLanguage.UpdatedAt = _currentDateUtcZero;
                        existingLanguage.UpdatedBy = _defaultUserId;

                        await _languageRepository.UpdateAsync(existingLanguage);
                    }
                }
                else
                {
                    newLanguage.CreatedAt = _currentDateUtcZero;
                    newLanguage.CreatedBy = _defaultUserId;

                    await _languageRepository.AddAsync(newLanguage);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Language)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Language)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateAuthorizationRoles(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Role)}");

            var authorizationRolesCSV = CsvImporter<Role>.FromCsvPath(path);
            var authorizationRolesDB = await _roleManager.Roles.ToListAsync();

            foreach (var newRole in authorizationRolesCSV)
            {
                var existingRole = authorizationRolesDB.FirstOrDefault(roleDB => roleDB.Id == newRole.Id);

                if (existingRole != null)
                {
                    if (existingRole.Name != newRole.Name)
                    {
                        existingRole.Name = newRole.Name;

                        existingRole.UpdatedAt = _currentDateUtcZero;
                        existingRole.UpdatedBy = _defaultUserId;

                        await _roleManager.UpdateAsync(existingRole);
                    }
                }
                else
                {
                    newRole.CreatedAt = _currentDateUtcZero;
                    newRole.CreatedBy = _defaultUserId;

                    await _roleManager.CreateAsync(newRole);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Role)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Role)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateCountries(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Country)}");

            var parsed = CsvImporter<Country>.FromCsvPath(path);
            var countries = await _countryRepository.ListAsync();

            foreach (var country in parsed)
            {
                var existingCountry = countries.FirstOrDefault(countries => countries.Id == country.Id);

                if (existingCountry != null)
                {
                    if (existingCountry.Name != country.Name || existingCountry.IsoCode != country.IsoCode ||
                        existingCountry.CountryCode != country.CountryCode ||
                        existingCountry.OfficialName != country.OfficialName)
                    {
                        existingCountry.Name = country.Name;
                        existingCountry.OfficialName = country.OfficialName;
                        existingCountry.IsoCode = country.IsoCode;
                        existingCountry.CountryCode = country.CountryCode;

                        existingCountry.UpdatedAt = _currentDateUtcZero;
                        existingCountry.UpdatedBy = _defaultUserId;

                        await _countryRepository.UpdateAsync(existingCountry);
                    }
                }
                else
                {
                    country.CreatedAt = _currentDateUtcZero;
                    country.CreatedBy = _defaultUserId;

                    await _countryRepository.AddAsync(country);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Country)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Country)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateTimeZones(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Core.Entities.TimeZones.TimeZone)}");

            var parsed = CsvImporter<Core.Entities.TimeZones.TimeZone>.FromCsvPath(path);
            var timeZones = await _timeZoneRepository.ListAsync();

            foreach (var timeZone in parsed)
            {
                var existingTimeZone = timeZones.FirstOrDefault(timeZones => timeZones.Id == timeZone.Id);

                if (existingTimeZone != null)
                {
                    if (existingTimeZone.Offset != timeZone.Offset || existingTimeZone.Text != timeZone.Text)
                    {
                        existingTimeZone.Offset = timeZone.Offset;
                        existingTimeZone.Text = timeZone.Text;

                        existingTimeZone.UpdatedAt = _currentDateUtcZero;
                        existingTimeZone.UpdatedBy = _defaultUserId;

                        await _timeZoneRepository.UpdateAsync(existingTimeZone);
                    }
                }
                else
                {
                    timeZone.CreatedAt = _currentDateUtcZero;
                    timeZone.CreatedBy = _defaultUserId;

                    await _timeZoneRepository.AddAsync(timeZone);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Core.Entities.TimeZones.TimeZone)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Core.Entities.TimeZones.TimeZone)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateJobRoleCategories(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(JobRoleCategory)}");

            var jobRoleCategoriesImporterCSV = CsvImporter<JobRoleCategoryImporter>.FromCsvPath(path).ToList();
            var jobRoleCategoriesCSV = _mapper.Map<List<JobRoleCategoryImporter>, List<JobRoleCategory>>(jobRoleCategoriesImporterCSV);

            var jobRoleCategoriesDB = await _jobRoleCategoryRepository.ListAsync();

            foreach (var newJobRoleCategory in jobRoleCategoriesCSV)
            {
                var existingJobRoleCategory = jobRoleCategoriesDB.FirstOrDefault(jobRoleCategoryDB => jobRoleCategoryDB.Id == newJobRoleCategory.Id);

                if (existingJobRoleCategory != null)
                {
                    if (existingJobRoleCategory.Name != newJobRoleCategory.Name || existingJobRoleCategory.Description != newJobRoleCategory.Description || existingJobRoleCategory.IsDefault != newJobRoleCategory.IsDefault)
                    {
                        existingJobRoleCategory.Name = newJobRoleCategory.Name;
                        existingJobRoleCategory.Description = newJobRoleCategory.Description;
                        existingJobRoleCategory.IsDefault = newJobRoleCategory.IsDefault;

                        existingJobRoleCategory.UpdatedAt = _currentDateUtcZero;
                        existingJobRoleCategory.UpdatedBy = _defaultUserId;

                        await _jobRoleCategoryRepository.UpdateAsync(existingJobRoleCategory);
                    }
                }
                else
                {
                    newJobRoleCategory.CreatedAt = _currentDateUtcZero;
                    newJobRoleCategory.CreatedBy = _defaultUserId;

                    await _jobRoleCategoryRepository.AddAsync(newJobRoleCategory);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(JobRoleCategory)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(JobRoleCategory)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateAppClients(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(AppClient)}");

            var appClientsCSV = CsvImporter<AppClient>.FromCsvPath(path);
            var appClientsDb = await _appClientRepository.ListAsync();

            foreach (var newAppClient in appClientsCSV)
            {
                var existingAppClient = appClientsDb.FirstOrDefault(jobRoleCategoryDB => jobRoleCategoryDB.Id == newAppClient.Id);

                if (existingAppClient != null)
                {
                    if (existingAppClient.AppName != newAppClient.AppName
                        || existingAppClient.Description != newAppClient.Description
                        || existingAppClient.ApiKey != newAppClient.ApiKey
                        || existingAppClient.ClientName != newAppClient.ClientName)
                    {
                        existingAppClient.AppName = newAppClient.AppName;
                        existingAppClient.Description = newAppClient.Description;
                        existingAppClient.ApiKey = newAppClient.ApiKey;
                        existingAppClient.ClientName = newAppClient.ClientName;

                        existingAppClient.UpdatedAt = _currentDateUtcZero;
                        existingAppClient.UpdatedBy = _defaultUserId;

                        await _appClientRepository.UpdateAsync(existingAppClient);
                    }
                }
                else
                {
                    newAppClient.CreatedAt = _currentDateUtcZero;
                    newAppClient.CreatedBy = _defaultUserId;

                    await _appClientRepository.AddAsync(newAppClient);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(AppClient)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(AppClient)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateIndustries(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Industry)}");

            var industriesCSV = CsvImporter<Industry>.FromCsvPath(path);
            var industriesDb = await _industryRepository.ListAsync();

            foreach (var newIndustry in industriesCSV)
            {
                var existingIndustry = industriesDb.FirstOrDefault(industryDB => industryDB.Id == newIndustry.Id);

                if (existingIndustry != null)
                {
                    if (existingIndustry.Name != newIndustry.Name)
                    {
                        existingIndustry.Name = newIndustry.Name;

                        existingIndustry.UpdatedAt = _currentDateUtcZero;
                        existingIndustry.UpdatedBy = _defaultUserId;

                        await _industryRepository.UpdateAsync(existingIndustry);
                    }
                }
                else
                {
                    newIndustry.CreatedAt = _currentDateUtcZero;
                    newIndustry.CreatedBy = _defaultUserId;

                    await _industryRepository.AddAsync(newIndustry);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Industry)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Industry)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateLanguageLevels(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(LanguageLevel)}");

            var languageLevelsCSV = CsvImporter<LanguageLevel>.FromCsvPath(path);
            var languageLevelsDb = await _languageLevelRepository.ListAsync();

            foreach (var newLanguageLevel in languageLevelsCSV)
            {
                var existingLanguageLevel = languageLevelsDb.FirstOrDefault(languageLvelDB => languageLvelDB.Id == newLanguageLevel.Id);

                if (existingLanguageLevel != null)
                {
                    if (existingLanguageLevel.Name != newLanguageLevel.Name || existingLanguageLevel.Description != newLanguageLevel.Description)
                    {
                        existingLanguageLevel.Name = newLanguageLevel.Name;
                        existingLanguageLevel.Description = newLanguageLevel.Description;

                        existingLanguageLevel.UpdatedAt = _currentDateUtcZero;
                        existingLanguageLevel.UpdatedBy = _defaultUserId;

                        await _languageLevelRepository.UpdateAsync(existingLanguageLevel);
                    }
                }
                else
                {
                    newLanguageLevel.CreatedAt = _currentDateUtcZero;
                    newLanguageLevel.CreatedBy = _defaultUserId;

                    await _languageLevelRepository.AddAsync(newLanguageLevel);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(LanguageLevel)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(LanguageLevel)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateGoalStatusCategory(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(GoalStatusCategory)}");

            var goalStatusCategoryCSV = CsvImporter<GoalStatusCategory>.FromCsvPath(path);
            var goalStatusCategoryDB = await _goalStatusCategoryRepository.ListAsync();

            foreach (var newGoalStatusCategory in goalStatusCategoryCSV)
            {
                var existingGoalStatusCategory = goalStatusCategoryDB.FirstOrDefault(pl => pl.Id == newGoalStatusCategory.Id);

                if (existingGoalStatusCategory != null)
                {
                    if (existingGoalStatusCategory.Name != newGoalStatusCategory.Name
                        || existingGoalStatusCategory.Description != newGoalStatusCategory.Description)
                    {
                        existingGoalStatusCategory.Name = newGoalStatusCategory.Name;
                        existingGoalStatusCategory.Description = newGoalStatusCategory.Description;

                        existingGoalStatusCategory.UpdatedAt = _currentDateUtcZero;
                        existingGoalStatusCategory.UpdatedBy = _defaultUserId;

                        await _goalStatusCategoryRepository.UpdateAsync(existingGoalStatusCategory);
                    }
                }
                else
                {
                    newGoalStatusCategory.CreatedAt = _currentDateUtcZero;
                    newGoalStatusCategory.CreatedBy = _defaultUserId;

                    await _goalStatusCategoryRepository.AddAsync(newGoalStatusCategory);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(GoalStatusCategory)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(GoalStatusCategory)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateGoalStatus(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(GoalStatus)}");

            var goalStatusCSV = CsvImporter<GoalStatus>.FromCsvPath(path);
            var goalStatusDB = await _goalStatusRepository.ListAsync();

            foreach (var newGoalStatus in goalStatusCSV)
            {
                var existingGoalStatus = goalStatusDB.FirstOrDefault(gs => gs.Id == newGoalStatus.Id);

                if (existingGoalStatus != null)
                {
                    if (existingGoalStatus.Name != newGoalStatus.Name
                        || existingGoalStatus.Description != newGoalStatus.Description
                        || existingGoalStatus.StatusCategoryId != newGoalStatus.StatusCategoryId
                        || existingGoalStatus.CompanyId != newGoalStatus.CompanyId
                        || existingGoalStatus.IsFinalStatus != newGoalStatus.IsFinalStatus)
                    {
                        existingGoalStatus.Name = newGoalStatus.Name;
                        existingGoalStatus.Description = newGoalStatus.Description;
                        existingGoalStatus.StatusCategoryId = newGoalStatus.StatusCategoryId;
                        existingGoalStatus.CompanyId = newGoalStatus.CompanyId;
                        existingGoalStatus.IsFinalStatus = newGoalStatus.IsFinalStatus;

                        existingGoalStatus.UpdatedAt = _currentDateUtcZero;
                        existingGoalStatus.UpdatedBy = _defaultUserId;

                        await _goalStatusRepository.UpdateAsync(existingGoalStatus);
                    }
                }
                else
                {
                    newGoalStatus.CreatedAt = _currentDateUtcZero;
                    newGoalStatus.CreatedBy = _defaultUserId;

                    await _goalStatusRepository.AddAsync(newGoalStatus);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(GoalStatus)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(GoalStatus)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    #endregion

    #region Execute Second
    private async Task CreateCities(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(City)}");

            var parsed = CsvImporter<City>.FromCsvPath(path);
            var cities = await _cityRepository.ListAsync();

            foreach (var city in parsed)
            {
                var existingCity = cities.FirstOrDefault(cities => cities.Id == city.Id);

                if (existingCity != null)
                {
                    if (existingCity.Name != city.Name || existingCity.IsoCode != city.IsoCode ||
                        existingCity.CountryId != city.CountryId)
                    {
                        existingCity.Name = city.Name;
                        existingCity.IsoCode = city.IsoCode;
                        existingCity.CountryId = city.CountryId;

                        existingCity.UpdatedAt = _currentDateUtcZero;
                        existingCity.UpdatedBy = _defaultUserId;

                        existingCity.Country = await _countryRepository.GetByIdAsync(city.CountryId);

                        await _cityRepository.UpdateAsync(existingCity);
                    }
                }
                else
                {
                    city.CreatedAt = _currentDateUtcZero;
                    city.CreatedBy = _defaultUserId;

                    city.Country = await _countryRepository.GetByIdAsync(city.CountryId);

                    await _cityRepository.AddAsync(city);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(City)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(City)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateJobRoles(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(JobRole)}");

            var jobRolesImporterCSV = CsvImporter<JobRoleImporter>.FromCsvPath(path);
            var jobRolesCSV = _mapper.Map<List<JobRoleImporter>, List<JobRole>>(jobRolesImporterCSV);
            var jobRolesDB = await _jobRoleRepository.ListAsync();

            foreach (var newJobRole in jobRolesCSV)
            {
                var existingJobRole = jobRolesDB.FirstOrDefault(jobRoleDB => jobRoleDB.Id == newJobRole.Id);

                if (existingJobRole != null)
                {
                    if (existingJobRole.Name != newJobRole.Name || existingJobRole.Description != newJobRole.Description || existingJobRole.IsDefault != newJobRole.IsDefault)
                    {
                        existingJobRole.Name = newJobRole.Name;
                        existingJobRole.Description = newJobRole.Description;
                        existingJobRole.JobRoleCategoryId = newJobRole.JobRoleCategoryId;
                        existingJobRole.IsDefault = newJobRole.IsDefault;

                        existingJobRole.UpdatedAt = _currentDateUtcZero;
                        existingJobRole.UpdatedBy = _defaultUserId;

                        existingJobRole.JobRoleCategory = await _jobRoleCategoryRepository.GetByIdAsync(existingJobRole.JobRoleCategoryId);

                        await _jobRoleRepository.UpdateAsync(existingJobRole);
                    }
                }
                else
                {
                    newJobRole.CreatedAt = _currentDateUtcZero;
                    newJobRole.CreatedBy = _defaultUserId;

                    newJobRole.JobRoleCategory = await _jobRoleCategoryRepository.GetByIdAsync(newJobRole.JobRoleCategoryId);

                    await _jobRoleRepository.AddAsync(newJobRole);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(JobRole)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(JobRole)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateBenefits(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Benefit)}");

            var benefitsImporterCSV = CsvImporter<BenefitImporter>.FromCsvPath(path).ToList();
            var benefitsCSV = _mapper.Map<List<BenefitImporter>, List<Benefit>>(benefitsImporterCSV);

            var benefitsDB = await _benefitRepository.ListAsync();

            foreach (var newBenefit in benefitsCSV)
            {
                var existingBenefits = benefitsDB.FirstOrDefault(benefitDB => benefitDB.Id == newBenefit.Id);

                if (existingBenefits != null)
                {
                    if (existingBenefits.Name != newBenefit.Name || existingBenefits.Description != newBenefit.Description)
                    {
                        existingBenefits.Name = newBenefit.Name;
                        existingBenefits.Description = newBenefit.Description;

                        existingBenefits.UpdatedAt = _currentDateUtcZero;
                        existingBenefits.UpdatedBy = _defaultUserId;

                        await _benefitRepository.UpdateAsync(existingBenefits);
                    }
                }
                else
                {
                    newBenefit.CreatedAt = _currentDateUtcZero;
                    newBenefit.CreatedBy = _defaultUserId;

                    await _benefitRepository.AddAsync(newBenefit);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Benefit)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Benefit)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateSkills(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Skill)}");

            var skillsCSV = CsvImporter<Skill>.FromCsvPath(path);
            var skillsDB = await _skillRepository.ListAsync();

            foreach (var newSkill in skillsCSV)
            {
                var existingSkill = skillsDB.FirstOrDefault(skillDB => skillDB.Id == newSkill.Id);

                if (existingSkill != null)
                {
                    if (existingSkill.Name != newSkill.Name || existingSkill.Description != newSkill.Description)
                    {
                        existingSkill.Name = newSkill.Name;
                        existingSkill.Description = newSkill.Description;

                        existingSkill.UpdatedAt = _currentDateUtcZero;
                        existingSkill.UpdatedBy = _defaultUserId;

                        await _skillRepository.UpdateAsync(existingSkill);
                    }
                }
                else
                {
                    newSkill.CreatedAt = _currentDateUtcZero;
                    newSkill.CreatedBy = _defaultUserId;

                    await _skillRepository.AddAsync(newSkill);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Skill)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Skill)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateSeniorities(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Seniority)}");

            var senioritiesCSV = CsvImporter<Seniority>.FromCsvPath(path);
            var senioritiesDB = await _seniorityRepository.ListAsync();

            foreach (var newSeniority in senioritiesCSV)
            {
                var existingSeniority = senioritiesDB.FirstOrDefault(seniorityDB => seniorityDB.Id == newSeniority.Id);

                if (existingSeniority != null)
                {
                    if (existingSeniority.Name != newSeniority.Name || existingSeniority.Description != newSeniority.Description)
                    {
                        existingSeniority.Name = newSeniority.Name;
                        existingSeniority.Description = newSeniority.Description;

                        existingSeniority.UpdatedAt = _currentDateUtcZero;
                        existingSeniority.UpdatedBy = _defaultUserId;

                        await _seniorityRepository.UpdateAsync(existingSeniority);
                    }
                }
                else
                {
                    newSeniority.CreatedAt = _currentDateUtcZero;
                    newSeniority.CreatedBy = _defaultUserId;

                    await _seniorityRepository.AddAsync(newSeniority);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Seniority)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Seniority)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateJobRoleCategorySkill(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(JobRoleCategorySkill)}");

            var jobRoleCategorySkillsImporter = CsvImporter<JobRoleCategorySkillImporter>.FromCsvPath(path);
            var jobRoleCategorySkillsCsv = _mapper.Map<List<JobRoleCategorySkillImporter>, List<JobRoleCategorySkill>>(jobRoleCategorySkillsImporter);

            var jobRoleCategorySkillDB = await _jobRoleCategorySkillRepository.ListAsync();

            foreach (var jobRoleCategory in jobRoleCategorySkillsCsv)
            {
                var existingJobRoleCategorySkill = jobRoleCategorySkillDB.FirstOrDefault(roleTranslationDB => roleTranslationDB.Id == jobRoleCategory.Id);

                if (existingJobRoleCategorySkill != null)
                {
                    if (existingJobRoleCategorySkill.SkillId != jobRoleCategory.SkillId || existingJobRoleCategorySkill.JobRoleCategoryId != jobRoleCategory.JobRoleCategoryId)
                    {
                        existingJobRoleCategorySkill.SkillId = jobRoleCategory.SkillId;
                        existingJobRoleCategorySkill.JobRoleCategoryId = jobRoleCategory.JobRoleCategoryId;


                        await _jobRoleCategorySkillRepository.UpdateAsync(existingJobRoleCategorySkill);
                    }
                }
                else
                {

                    jobRoleCategory.CreatedAt = _currentDateUtcZero;
                    jobRoleCategory.CreatedBy = _defaultUserId;

                    await _jobRoleCategorySkillRepository.AddAsync(jobRoleCategory);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(JobRoleCategorySkill)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(JobRoleCategorySkill)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreatePolicies(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(Policy)}");

            var PoliciesImporter = CsvImporter<PolicyImporter>.FromCsvPath(path);
            var PoliciesCsv = _mapper.Map<List<PolicyImporter>, List<Policy>>(PoliciesImporter);

            var PoliciesDB = await _policyRepository.ListAsync();

            foreach (var policy in PoliciesCsv)
            {
                var existingPolicy = PoliciesDB.FirstOrDefault(policyDB => policyDB.Id == policy.Id);

                if (existingPolicy != null)
                {
                    if (existingPolicy.Name != policy.Name || existingPolicy.Description != policy.Description)
                    {
                        existingPolicy.Name = policy.Name;
                        existingPolicy.Description = policy.Description;

                        existingPolicy.UpdatedAt = _currentDateUtcZero;
                        existingPolicy.UpdatedBy = _defaultUserId;

                        await _policyRepository.UpdateAsync(existingPolicy);
                    }
                }
                else
                {
                    policy.CreatedAt = _currentDateUtcZero;
                    policy.CreatedBy = _defaultUserId;

                    await _policyRepository.AddAsync(policy);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(Policy)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(Policy)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateContractType(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(ContractType)}");

            var contractTypesImporter = CsvImporter<ContractTypeImporter>.FromCsvPath(path);
            var contractTypesCsv = _mapper.Map<List<ContractTypeImporter>, List<ContractType>>(contractTypesImporter);

            var contractTypesDB = await _contractTypeRepository.ListAsync();

            foreach (var contractType in contractTypesCsv)
            {
                var existingContractType = contractTypesDB.FirstOrDefault(contractTypeDb => contractTypeDb.Id == contractType.Id);

                if (existingContractType != null)
                {
                    if (existingContractType.Name != contractType.Name || existingContractType.Description != contractType.Description)
                    {
                        existingContractType.Name = contractType.Name;
                        existingContractType.Description = contractType.Description;

                        existingContractType.UpdatedAt = _currentDateUtcZero;
                        existingContractType.UpdatedBy = _defaultUserId;

                        await _contractTypeRepository.UpdateAsync(existingContractType);
                    }
                }
                else
                {
                    contractType.CreatedAt = _currentDateUtcZero;
                    contractType.CreatedBy = _defaultUserId;

                    await _contractTypeRepository.AddAsync(contractType);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(ContractType)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(ContractType)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreateStoryPoints(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(StoryPoint)}");

            var storyPointsCSV = CsvImporter<StoryPoint>.FromCsvPath(path);
            var storyPointsDB = await _storyPointRepository.ListAsync();

            foreach (var newStoryPoint in storyPointsCSV)
            {
                var existingStoryPoint = storyPointsDB.FirstOrDefault(sp => sp.Id == newStoryPoint.Id);

                if (existingStoryPoint != null)
                {
                    if (existingStoryPoint.KnowledgeLevel != newStoryPoint.KnowledgeLevel
                        || existingStoryPoint.Dependencies != newStoryPoint.Dependencies
                        || existingStoryPoint.WorkEffort != newStoryPoint.WorkEffort
                        || existingStoryPoint.Points != newStoryPoint.Points)
                    {
                        existingStoryPoint.KnowledgeLevel = newStoryPoint.KnowledgeLevel;
                        existingStoryPoint.Dependencies = newStoryPoint.Dependencies;
                        existingStoryPoint.WorkEffort = newStoryPoint.WorkEffort;
                        existingStoryPoint.Points = newStoryPoint.Points;

                        existingStoryPoint.UpdatedAt = _currentDateUtcZero;
                        existingStoryPoint.UpdatedBy = _defaultUserId;

                        await _storyPointRepository.UpdateAsync(existingStoryPoint);
                    }
                }
                else
                {
                    newStoryPoint.CreatedAt = _currentDateUtcZero;
                    newStoryPoint.CreatedBy = _defaultUserId;

                    await _storyPointRepository.AddAsync(newStoryPoint);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(StoryPoint)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(StoryPoint)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    private async Task CreatePriorityLevels(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(PriorityLevel)}");

            var priorityLevelsCSV = CsvImporter<PriorityLevel>.FromCsvPath(path);
            var priorityLevelsDB = await _priorityLevelRepository.ListAsync();

            foreach (var newPriorityLevel in priorityLevelsCSV)
            {
                var existingPriorityLevel = priorityLevelsDB.FirstOrDefault(pl => pl.Id == newPriorityLevel.Id);

                if (existingPriorityLevel != null)
                {
                    if (existingPriorityLevel.Level != newPriorityLevel.Level
                        || existingPriorityLevel.Description != newPriorityLevel.Description
                        || existingPriorityLevel.HexColorCode != newPriorityLevel.HexColorCode)
                    {
                        existingPriorityLevel.Level = newPriorityLevel.Level;
                        existingPriorityLevel.Description = newPriorityLevel.Description;
                        existingPriorityLevel.HexColorCode = newPriorityLevel.HexColorCode;

                        existingPriorityLevel.UpdatedAt = _currentDateUtcZero;
                        existingPriorityLevel.UpdatedBy = _defaultUserId;

                        await _priorityLevelRepository.UpdateAsync(existingPriorityLevel);
                    }
                }
                else
                {
                    newPriorityLevel.CreatedAt = _currentDateUtcZero;
                    newPriorityLevel.CreatedBy = _defaultUserId;

                    await _priorityLevelRepository.AddAsync(newPriorityLevel);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(PriorityLevel)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(PriorityLevel)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    #endregion

    #region Execute Last | Multilenguages
    private async Task CreateAuthorizationRolesTranslations(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(RoleTranslation)}");

            var authorizationRolesTranslationsCSV = CsvImporter<RoleTranslation>.FromCsvPath(path);
            var authorizationRolesTranslationsDB = await _roleTranslationRepository.ListAsync();

            foreach (var newRoleTranslation in authorizationRolesTranslationsCSV)
            {
                var existingRoleTranslation = authorizationRolesTranslationsDB.FirstOrDefault(roleTranslationDB => roleTranslationDB.Id == newRoleTranslation.Id);

                if (existingRoleTranslation != null)
                {
                    if (existingRoleTranslation.Name != newRoleTranslation.Name)
                    {
                        existingRoleTranslation.Name = newRoleTranslation.Name;
                        existingRoleTranslation.Language = await _languageRepository.GetByIdAsync(newRoleTranslation.LanguageId);
                        existingRoleTranslation.Role = await _roleManager.FindByIdAsync(newRoleTranslation.RoleId);

                        existingRoleTranslation.UpdatedAt = _currentDateUtcZero;
                        existingRoleTranslation.UpdatedBy = _defaultUserId;

                        await _roleTranslationRepository.UpdateAsync(existingRoleTranslation);
                    }
                }
                else
                {
                    newRoleTranslation.Language = await _languageRepository.GetByIdAsync(newRoleTranslation.LanguageId);
                    newRoleTranslation.Role = await _roleManager.FindByIdAsync(newRoleTranslation.RoleId);

                    newRoleTranslation.CreatedAt = _currentDateUtcZero;
                    newRoleTranslation.CreatedBy = _defaultUserId;

                    await _roleTranslationRepository.AddAsync(newRoleTranslation);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(RoleTranslation)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(RoleTranslation)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    //Not implemented
    private async Task CreateWorkRoleTranslation(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(JobRoleTranslation)}");

            var workRoleTranslationsCSV = CsvImporter<JobRoleTranslation>.FromCsvPath(path);
            var workRoleTranslationsDB = await _jobRoleTranslationRepository.ListAsync();

            foreach (var newWorkRoleTranslation in workRoleTranslationsCSV)
            {
                var existingWorkRoleTranslation = workRoleTranslationsDB.FirstOrDefault(workRoleTranslationDB => workRoleTranslationDB.Id == newWorkRoleTranslation.Id);

                if (existingWorkRoleTranslation != null)
                {
                    if (existingWorkRoleTranslation.Name != newWorkRoleTranslation.Name || existingWorkRoleTranslation.Description != newWorkRoleTranslation.Description ||
                        existingWorkRoleTranslation.JobRoleId != newWorkRoleTranslation.JobRoleId || existingWorkRoleTranslation.LanguageId != newWorkRoleTranslation.LanguageId)
                    {
                        existingWorkRoleTranslation.Name = newWorkRoleTranslation.Name;
                        existingWorkRoleTranslation.Description = newWorkRoleTranslation.Description;
                        existingWorkRoleTranslation.LanguageId = newWorkRoleTranslation.LanguageId;
                        existingWorkRoleTranslation.JobRoleId = newWorkRoleTranslation.JobRoleId;

                        existingWorkRoleTranslation.UpdatedAt = _currentDateUtcZero;
                        existingWorkRoleTranslation.UpdatedBy = _defaultUserId;

                        await _jobRoleTranslationRepository.UpdateAsync(existingWorkRoleTranslation);
                    }
                }
                else
                {
                    newWorkRoleTranslation.CreatedAt = _currentDateUtcZero;
                    newWorkRoleTranslation.CreatedBy = _defaultUserId;

                    await _jobRoleTranslationRepository.AddAsync(newWorkRoleTranslation);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(JobRoleTranslation)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(JobRoleTranslation)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }

    //Not implemented
    private async Task CreateWorkRoleCategoryTranslation(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(JobRoleCategoryTranslation)}");

            var workRoleCategoryTranslationsCSV = CsvImporter<JobRoleCategoryTranslation>.FromCsvPath(path);
            var workRoleCategoryTranslationsDB = await _jobRoleCategoryTranslationRepository.ListAsync();

            foreach (var newWorkRoleCategoryTranslation in workRoleCategoryTranslationsCSV)
            {
                var existingWorkRoleCategoryTranslation = workRoleCategoryTranslationsDB.FirstOrDefault(workRoleTranslationDB => workRoleTranslationDB.Id == newWorkRoleCategoryTranslation.Id);

                if (existingWorkRoleCategoryTranslation != null)
                {
                    if (existingWorkRoleCategoryTranslation.Name != newWorkRoleCategoryTranslation.Name || existingWorkRoleCategoryTranslation.Description != newWorkRoleCategoryTranslation.Description ||
                        existingWorkRoleCategoryTranslation.JobRoleCategoryId != newWorkRoleCategoryTranslation.JobRoleCategoryId || existingWorkRoleCategoryTranslation.LanguageId != newWorkRoleCategoryTranslation.LanguageId)
                    {
                        existingWorkRoleCategoryTranslation.Name = newWorkRoleCategoryTranslation.Name;
                        existingWorkRoleCategoryTranslation.Description = newWorkRoleCategoryTranslation.Description;
                        existingWorkRoleCategoryTranslation.LanguageId = newWorkRoleCategoryTranslation.LanguageId;
                        existingWorkRoleCategoryTranslation.JobRoleCategoryId = newWorkRoleCategoryTranslation.JobRoleCategoryId;

                        existingWorkRoleCategoryTranslation.UpdatedAt = _currentDateUtcZero;
                        existingWorkRoleCategoryTranslation.UpdatedBy = _defaultUserId;

                        await _jobRoleCategoryTranslationRepository.UpdateAsync(existingWorkRoleCategoryTranslation);
                    }
                }
                else
                {
                    newWorkRoleCategoryTranslation.CreatedAt = _currentDateUtcZero;
                    newWorkRoleCategoryTranslation.CreatedBy = _defaultUserId;

                    await _jobRoleCategoryTranslationRepository.AddAsync(newWorkRoleCategoryTranslation);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(JobRoleCategoryTranslation)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(JobRoleCategoryTranslation)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }
    private async Task CreatePriorityLevelTranslations(string path)
    {
        try
        {
            _logger.LogInformation($"Start of the process of importing seed values for {nameof(PriorityLevelTranslation)}");

            var priorityLevelTranslationsCSV = CsvImporter<PriorityLevelTranslation>.FromCsvPath(path);
            var priorityLevelTranslationsDB = await _priorityLevelTranslationRepository.ListAsync();

            foreach (var newPriorityLevelTranslation in priorityLevelTranslationsCSV)
            {
                var existingPriorityLevelTranslation = priorityLevelTranslationsDB.FirstOrDefault(pl => pl.Id == newPriorityLevelTranslation.Id);

                if (existingPriorityLevelTranslation != null)
                {
                    existingPriorityLevelTranslation.Language = null;
                    existingPriorityLevelTranslation.PriorityLevel = null;

                    if (existingPriorityLevelTranslation.Level != newPriorityLevelTranslation.Level
                        || existingPriorityLevelTranslation.Description != newPriorityLevelTranslation.Description
                        || existingPriorityLevelTranslation.PriorityLevelId != newPriorityLevelTranslation.PriorityLevelId
                        || existingPriorityLevelTranslation.LanguageId != newPriorityLevelTranslation.LanguageId)
                    {
                        existingPriorityLevelTranslation.Level = newPriorityLevelTranslation.Level;
                        existingPriorityLevelTranslation.Description = newPriorityLevelTranslation.Description;
                        existingPriorityLevelTranslation.PriorityLevelId = newPriorityLevelTranslation.PriorityLevelId;
                        existingPriorityLevelTranslation.LanguageId = newPriorityLevelTranslation.LanguageId;

                        existingPriorityLevelTranslation.UpdatedAt = _currentDateUtcZero;
                        existingPriorityLevelTranslation.UpdatedBy = _defaultUserId;

                        await _priorityLevelTranslationRepository.UpdateAsync(existingPriorityLevelTranslation);
                    }
                }
                else
                {
                    newPriorityLevelTranslation.Language = await _languageRepository.GetByIdAsync(newPriorityLevelTranslation.LanguageId);
                    newPriorityLevelTranslation.PriorityLevel = await _priorityLevelRepository.GetByIdAsync(newPriorityLevelTranslation.PriorityLevelId);

                    newPriorityLevelTranslation.CreatedAt = _currentDateUtcZero;
                    newPriorityLevelTranslation.CreatedBy = _defaultUserId;

                    await _priorityLevelTranslationRepository.AddAsync(newPriorityLevelTranslation);
                }
            }

            _logger.LogInformation($"End of the process of importing seed values for {nameof(PriorityLevelTranslation)}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in the process of importing seed values for {nameof(PriorityLevelTranslation)} for the following reason {ErrorHelper.GetExceptionError(ex)}");
        }
    }
    #endregion
}