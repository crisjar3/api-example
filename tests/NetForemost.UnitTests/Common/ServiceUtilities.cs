using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NetForemost.Core.Entities.AppClients;
using NetForemost.Core.Entities.Authentication;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.CDN;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Emails;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.Core.Entities.Languages;
using NetForemost.Core.Entities.Policies;
using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Seniorities;
using NetForemost.Core.Entities.Skills;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Entities.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Account;
using NetForemost.Core.Interfaces.Authentication;
using NetForemost.Core.Interfaces.Companies;
using NetForemost.Core.Interfaces.Email;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.Core.Interfaces.SendGrid;
using NetForemost.Core.Interfaces.Settings;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.Core.Services.Account;
using NetForemost.Core.Services.Authentication;
using NetForemost.Core.Services.Benefits;
using NetForemost.Core.Services.Cities;
using NetForemost.Core.Services.Companies;
using NetForemost.Core.Services.ContractTypes;
using NetForemost.Core.Services.Countries;
using NetForemost.Core.Services.Dashboards;
using NetForemost.Core.Services.Email;
using NetForemost.Core.Services.Goals;
using NetForemost.Core.Services.Images;
using NetForemost.Core.Services.Industries;
using NetForemost.Core.Services.JobOffers;
using NetForemost.Core.Services.JobRoles;
using NetForemost.Core.Services.Languages;
using NetForemost.Core.Services.Policies;
using NetForemost.Core.Services.PriorityLevels;
using NetForemost.Core.Services.Projects;
using NetForemost.Core.Services.Seniorities;
using NetForemost.Core.Services.Settings;
using NetForemost.Core.Services.Skills;
using NetForemost.Core.Services.StoryPoints;
using NetForemost.Core.Services.TalentsPool;
using NetForemost.Core.Services.Tasks;
using NetForemost.Core.Services.TimeTrackings;
using NetForemost.SharedKernel.Interfaces;
using AuthenticationService = NetForemost.Core.Services.Authentication.AuthenticationService;
using Task = NetForemost.Core.Entities.Tasks.Task;
using TaskEntity = NetForemost.Core.Entities.Tasks.Task;

namespace NetForemost.UnitTests.Common;

/// <summary>
/// Service Initializer
/// </summary>
/// 

public class ServiceUtilities
{
    public static TaskTypeService CreateTaskTypeService(
     out Mock<IAsyncRepository<Company>> companyRepository,
     out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
     out Mock<IAsyncRepository<Project>> projectRepository,
     out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
     out Mock<IAsyncRepository<Task>> taskRepository
     )
    {
        companyRepository = new();
        taskTypeRepository = new();
        companyUserRepository = new();
        projectRepository = new();
        taskRepository = new();

        var taskTypeService = new TaskTypeService(companyRepository.Object, companyUserRepository.Object,
                                            projectRepository.Object, taskTypeRepository.Object, taskRepository.Object);
        return taskTypeService;
    }

    public static TaskService TaskService(
        out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
        out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
        out Mock<ITaskTypeService> taskTypeService,
        out Mock<IGoalService> goalService,
        out Mock<UserManager<User>> userManager,
        out Mock<IAsyncRepository<Goal>> goalRepository
        )
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        userManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        taskRepository = new();
        taskTypeRepository = new();
        taskTypeService = new();
        goalService = new();
        goalRepository = new();

        var taskService = new TaskService(taskRepository.Object, taskTypeRepository.Object,
                                            taskTypeService.Object, goalService.Object, userManager.Object, goalRepository.Object);
        return taskService;
    }

    public static ProjectService CreateProjectServices(

        out Mock<IAsyncRepository<Company>> companyRepository,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out Mock<IAsyncRepository<Project>> projectRepository,
        out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
        out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
        out Mock<IAsyncRepository<ProjectAvatar>> projectAvatarRepository,
        out Mock<StorageClient> storageClient,
        out Mock<IOptions<CloudStorageConfig>> config
        )
    {
        companyRepository = new();
        companyUserRepository = new();
        projectRepository = new();
        projectCompanyUserRepository = new();
        jobRoleRepository = new();
        projectAvatarRepository = new();

        storageClient = new();
        config = new Mock<IOptions<CloudStorageConfig>>();
        config.Setup(config => config.Value).Returns(new CloudStorageConfig()
        {
            BucketName = "netforemost-avatar-bucket",
            BaseUrl = "https://storage.googleapis.com/netforemost-avatar-bucket/"
        });

        var projectService = new ProjectService(companyRepository.Object, companyUserRepository.Object,
                                                projectRepository.Object, projectCompanyUserRepository.Object,
                                                jobRoleRepository.Object, projectAvatarRepository.Object, storageClient.Object, config.Object);
        return projectService;
    }

    public static TalentPoolService CreateTalentPoolService(out Mock<UserManager<User>> userManager)
    {

        var userStoreMocku = new Mock<IUserStore<User>>();
        userManager = new Mock<UserManager<User>>(userStoreMocku.Object, null, null, null, null, null, null, null, null);


        var talentPoolService = new TalentPoolService(userManager.Object);

        return talentPoolService;

    }


    public static CompanySettingsService CreateCompanySettingsService(
        out Mock<ICompanyService> companyService,
        out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
        )
    {
        companyService = new Mock<ICompanyService>();
        companySettingsRepository = new Mock<IAsyncRepository<CompanySettings>>();

        var companySettingsService = new CompanySettingsService(
            companyService.Object,
            companySettingsRepository.Object);

        return companySettingsService;
    }

    public static CompanyService CreateCompanyService(

                out Mock<IAsyncRepository<City>> cityRepository,
                out Mock<IAsyncRepository<Company>> companyRepository,
                out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
                out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
                out Mock<RoleManager<Role>> roleManager,
                out Mock<IAsyncRepository<Industry>> industryRepository
                )
    {
        var roleStoreMock = new Mock<IRoleStore<Role>>();
        companyRepository = new();
        cityRepository = new();
        timeZoneRepository = new();
        companyUserRepository = new();
        roleManager = new Mock<RoleManager<Role>>(roleStoreMock.Object, null, null, null, null);
        industryRepository = new();

        var companyService = new CompanyService(companyRepository.Object, cityRepository.Object,
            timeZoneRepository.Object, companyUserRepository.Object, roleManager.Object,
            industryRepository.Object);

        return companyService;
    }

    public static CityService CreateCityService(
        out Mock<IAsyncRepository<City>> cityRepository,
        out Mock<IAsyncRepository<Country>> countryRepository)
    {
        cityRepository = new();
        countryRepository = new();

        var companyService = new CityService(cityRepository.Object, countryRepository.Object);

        return companyService;
    }

    public static GoalService ConfirmGoalService(
        out Mock<IAsyncRepository<Goal>> goalRepository,
        out Mock<IAsyncRepository<GoalExtraMile>> extraMileGoalRepository,
        out Mock<IAsyncRepository<Project>> projectRepository,
        out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
        out Mock<UserManager<User>> userManager,
        Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository = null
        )

    {
        var userStoreMock = new Mock<IUserStore<User>>();
        userManager = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);


        goalRepository = new();
        extraMileGoalRepository = new();
        projectRepository = new();
        goalStatusRepository = new();
        projectCompanyUserRepository ??= new Mock<IAsyncRepository<ProjectCompanyUser>>();

        var priorityLevelReposiotry = new Mock<IAsyncRepository<PriorityLevel>>();
        var storyPointRepository = new Mock<IAsyncRepository<StoryPoint>>();

        var goalService = new GoalService(
            goalRepository.Object,
            extraMileGoalRepository.Object,
            projectRepository.Object,
            goalStatusRepository.Object,
            userManager.Object,
            priorityLevelReposiotry.Object,
            storyPointRepository.Object,
            projectCompanyUserRepository.Object
            );

        return goalService;
    }

    public static GoalService CreateGoalService(
    out Mock<IAsyncRepository<Goal>> goalRepository,
    out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMileRepository,
    out Mock<IAsyncRepository<Project>> projectRepository,
    out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
    out Mock<UserManager<User>> userManager,
    out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
    Mock<IAsyncRepository<PriorityLevel>> priorityLevelRepository = null,
    Mock<IAsyncRepository<StoryPoint>> storyPointRepository = null
    )
    {
        goalRepository = new();
        goalExtraMileRepository = new();
        projectRepository = new();
        goalStatusRepository = new();
        priorityLevelRepository ??= new Mock<IAsyncRepository<PriorityLevel>>();
        storyPointRepository ??= new Mock<IAsyncRepository<StoryPoint>>();
        projectCompanyUserRepository = new();
        var userStoreMock = new Mock<IUserStore<User>>();
        userManager = new(userStoreMock.Object, null, null, null, null, null, null, null, null);

        var goalService = new GoalService(
            goalRepository.Object,
            goalExtraMileRepository.Object,
            projectRepository.Object,
            goalStatusRepository.Object,
            userManager.Object,
            priorityLevelRepository.Object,
            storyPointRepository.Object,
            projectCompanyUserRepository.Object
        );

        return goalService;
    }


    public static AccountService CreateHireUserService(
        out Mock<IAsyncRepository<City>> cityRepository,
        out Mock<IAsyncRepository<Skill>> skillRepository,
        out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
        out Mock<IAsyncRepository<Seniority>> seniorityRepository,
        out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
        out Mock<IAsyncRepository<Language>> languageRepository,
        out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository,
        out Mock<IEmailService> emailService,
        out Mock<UserManager<User>> userManager,
        out Mock<RoleManager<Role>> roleManager,
        Mock<IAsyncRepository<UserSettings>> userSettingsRepository = null,
        Mock<IUserSettingsService> userSettingsServices = null

        )
    {
        var userStoreMock = new Mock<IUserStore<User>>();

        userManager = new Mock<UserManager<User>>(
            userStoreMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
            );

        cityRepository = new();
        skillRepository = new();
        jobRoleRepository = new();
        seniorityRepository = new();
        timeZoneRepository = new();
        languageRepository = new();
        languageLevelRepository = new();
        emailService = new();
        userSettingsRepository ??= new Mock<IAsyncRepository<UserSettings>>();
        userSettingsServices ??= new Mock<IUserSettingsService>();

        var roleStoreMock = new Mock<IQueryableRoleStore<Role>>();
        var store = new Mock<IRoleStore<Role>>();
        var roleValidators = new Mock<IEnumerable<IRoleValidator<Role>>>();
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new IdentityErrorDescriber();
        var logger = new Mock<ILogger<RoleManager<Role>>>();
        //roleManager = new Mock<RoleManager<Role>>(roleStoreMock.Object, roleValidators.Object, keyNormalizer.Object, errors, logger);
        roleManager = new Mock<RoleManager<Role>>(roleStoreMock.Object, null, null, null, null);

        var accountService = new AccountService(
            cityRepository.Object,
            skillRepository.Object,
            jobRoleRepository.Object,
            seniorityRepository.Object,
            timeZoneRepository.Object,
            languageRepository.Object,
            languageLevelRepository.Object,
            emailService.Object,
            userManager.Object,
            userSettingsRepository.Object,
            userSettingsServices.Object,
            roleManager.Object
            );

        return accountService;
    }

    public static AuthenticationService CreateAuthenticacionService(
        out Mock<UserManager<User>> userManager,
        out Mock<SignInManager<User>> signInManager,
        out Mock<IAsyncRepository<UserRefreshToken>> userRefreshTokenRepository,
        out Mock<IAccountService> accountService,
        out Mock<ITokenManagerService> tokenManagerService,
        out Mock<IOptions<JwtConfig>> jwtConfig,
        out Mock<IAsyncRepository<AppClient>> appClientRepository
)
    {
        var config = new Mock<IOptions<JwtConfig>>();

        config.Setup(config => config.Value).Returns(new JwtConfig()
        {
            AuthTokenValidityInMins = 1440,
            RefreshTokenValidityInDays = 1,
        });

        userManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
            );

        signInManager = new Mock<SignInManager<User>>(
            userManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<User>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object
            );


        userRefreshTokenRepository = new();
        accountService = new();
        tokenManagerService = new();
        jwtConfig = config;
        appClientRepository = new();

        var authenticationService = new AuthenticationService(
            userManager.Object,
            signInManager.Object,
            userRefreshTokenRepository.Object,
            accountService.Object,
            tokenManagerService.Object,
            jwtConfig.Object,
            appClientRepository.Object
            );

        return authenticationService;
    }

    public static TokenManagerService CreateTokenManagerService(
        out Mock<IHttpContextAccessor> httpContextAccessor,
        out Mock<IOptions<JwtConfig>> jwtConfig,
        out Mock<StackExchange.Redis.IConnectionMultiplexer> redisConection,
        out Mock<IConfiguration> configuration
        )
    {
        var config = new Mock<IOptions<JwtConfig>>();

        config.Setup(config => config.Value).Returns(new JwtConfig()
        {
            AuthTokenValidityInMins = 1440,
            RefreshTokenValidityInDays = 1,
        });

        httpContextAccessor = new();
        jwtConfig = config;
        redisConection = new();
        configuration = new();

        var tokenManagerService = new TokenManagerService(
            httpContextAccessor.Object,
            jwtConfig.Object,
            redisConection.Object,
            configuration.Object
            );

        return tokenManagerService;
    }
    public static EmailService CreateEmailService(
        out Mock<IOptions<SmtpClientConfig>> options,
        out Mock<ISmtpClient> smtp
    )
    {
        var config = new Mock<IOptions<SmtpClientConfig>>();
        config.Setup(config => config.Value).Returns(new SmtpClientConfig()
        {
            Host = "smtp.gmail.com",
            Port = 465,
            EnableSsl = true,
            UseDefaultCredentials = false,
            UserName = "fernandoj.medinaa@gmail.com",
            Password = "password",

        });

        options = config;
        smtp = new();

        var emailService = new EmailService(options.Object, smtp.Object);

        return emailService;
    }

    public static JobOfferService CreateJobOfferService(
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
        )
    {
        cityRepository = new();
        countryRepository = new();
        companyRepository = new();
        benefitRepository = new();
        jobRoleRepository = new();
        seniorityRepository = new();
        skillRepository = new();
        jobOfferRepository = new();
        policyRepository = new();
        contractTypeRepository = new();
        languageRepository = new();
        languageLevelRepository = new();
        projectRepository = new();

        var jobOfferService = new JobOfferService(
            cityRepository.Object,
            countryRepository.Object,
            companyRepository.Object,
            benefitRepository.Object,
            jobRoleRepository.Object,
            seniorityRepository.Object,
            skillRepository.Object,
            jobOfferRepository.Object,
            policyRepository.Object,
            contractTypeRepository.Object,
            languageRepository.Object,
            languageLevelRepository.Object,
            projectRepository.Object
            );

        return jobOfferService;
    }

    public static CompanyUserService CreateCompanyUserService(
        out Mock<IAsyncRepository<Company>> companyRepository,
        out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out Mock<UserManager<User>> userManager,
        out Mock<RoleManager<Role>> roleManager,
        out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
        out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
        out Mock<IAsyncRepository<Project>> projectRepository
        )
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var roleStoreMock = new Mock<IRoleStore<Role>>();

        userManager = new Mock<UserManager<User>>(
            userStoreMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
            );

        roleManager = new Mock<RoleManager<Role>>(
            roleStoreMock.Object,
            new IRoleValidator<Role>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<Role>>>().Object
            );

        companyRepository = new();
        companyUserRepository = new();
        timeZoneRepository = new();
        jobRoleRepository = new();
        projectCompanyUserRepository = new();
        projectRepository = new();

        var companyUserService = new CompanyUserService(
            companyRepository.Object,
            timeZoneRepository.Object,
            companyUserRepository.Object,
            userManager.Object,
            roleManager.Object,
            jobRoleRepository.Object,
            projectCompanyUserRepository.Object,
            projectRepository.Object
            );

        return companyUserService;
    }

    public static CompanyUserService UpdateCompanyUserService(
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<RoleManager<Role>> roleManager,
            out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
            out Mock<IAsyncRepository<Project>> projectRepository
        )
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var roleStoreMock = new Mock<IRoleStore<Role>>();

        userManager = new Mock<UserManager<User>>(
            userStoreMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
            );

        roleManager = new Mock<RoleManager<Role>>(
            roleStoreMock.Object,
            new IRoleValidator<Role>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<ILogger<RoleManager<Role>>>().Object
            );

        companyRepository = new();
        companyUserRepository = new();
        timeZoneRepository = new();
        jobRoleRepository = new();
        projectCompanyUserRepository = new();
        projectRepository = new();

        var companyUserService = new CompanyUserService(
            companyRepository.Object,
            timeZoneRepository.Object,
            companyUserRepository.Object,
            userManager.Object,
            roleManager.Object,
            jobRoleRepository.Object,
            projectCompanyUserRepository.Object,
            projectRepository.Object
        );

        return companyUserService;
    }

    public static CountryService CreateCountryService(out Mock<IAsyncRepository<Country>> countryRepository)
    {
        countryRepository = new();

        var countryService = new CountryService(countryRepository.Object);

        return countryService;
    }

    public static ImageStorageService CreateImageStorageService(
        out Mock<StorageClient> storageClient,
        out Mock<IOptions<CloudStorageConfig>> config
        )
    {
        storageClient = new();
        config = new Mock<IOptions<CloudStorageConfig>>();
        config.Setup(config => config.Value).Returns(new CloudStorageConfig()
        {
            BucketName = "netforemost-avatar-bucket",
            BaseUrl = "https://storage.googleapis.com/netforemost-avatar-bucket/"
        });

        var imageStorageService = new ImageStorageService(storageClient.Object, config.Object);

        return imageStorageService;
    }

    public static IndustryService CreateIndustryService(out Mock<IAsyncRepository<Industry>> industryRepository)
    {
        industryRepository = new();

        var industryService = new IndustryService(industryRepository.Object);

        return industryService;
    }

    public static bool CompareList(object[] valuesA, object[] valuesB)
    {
        for (int i = 0; i < valuesA.Length; i++)
        {
            if (!valuesA[i].Equals(valuesB[i])) return false;
        }

        return true;
    }

    public static JobRoleService CreateJobRoleService(
        out Mock<IAsyncRepository<JobRoleCategory>> jobRoleCategoryRepository,
        out Mock<IAsyncRepository<Company>> companyRepository,
        out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository
        )
    {
        jobRoleCategoryRepository = new();
        companyRepository = new();
        jobRoleRepository = new();
        companyUserRepository = new();

        var jobRoleService = new JobRoleService(
            jobRoleCategoryRepository.Object,
            companyRepository.Object,
            jobRoleRepository.Object,
            companyUserRepository.Object
            );

        return jobRoleService;
    }

    public static DashboardService CreateDashboardService(
        out Mock<IAsyncRepository<JobOffer>> jobOfferRepository,
        out Mock<IAsyncRepository<Project>> projectRepository,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository)
    {
        jobOfferRepository = new();
        projectRepository = new();
        companyUserRepository = new();

        var dashboardService = new DashboardService(
            jobOfferRepository.Object,
            projectRepository.Object,
            companyUserRepository.Object
            );

        return dashboardService;
    }

    public static bool CompareDictionary(Dictionary<string, int> valuesA, Dictionary<string, int> valuesB)
    {
        foreach (var _ in valuesA.Where(valueA => !valuesB.Contains(valueA) && valuesA.Count == valuesB.Count).Select(valueA => new { })) return false;

        return true;
    }

    public static TimerService CreateTimerService(
        out Mock<IAsyncRepository<TaskEntity>> taskRepository,
        out Mock<IAsyncRepository<DailyTimeBlock>> blockTimeTrackingRepository,
        out Mock<IAsyncRepository<Goal>> goalRepository,
        Mock<UserManager<User>> userManager)
    {
        taskRepository = new();
        blockTimeTrackingRepository = new();
        userManager = new();
        var userStoreMock = new Mock<IUserStore<User>>();
        goalRepository = new();
        userManager = new(userStoreMock.Object, null, null, null, null, null, null, null, null);

        var dashboardService = new TimerService(
            taskRepository.Object,
            blockTimeTrackingRepository.Object,
            userManager.Object,
            goalRepository.Object
            );

        return dashboardService;
    }

    public static BenefitService CreateBenefitService(
        out Mock<IAsyncRepository<Benefit>> benefitsRepository,
        out Mock<IAsyncRepository<Company>> companyRepository
        )
    {
        benefitsRepository = new();
        companyRepository = new();

        var benefitsService = new BenefitService(
            benefitsRepository.Object,
            companyRepository.Object
            );

        return benefitsService;
    }

    public static ContractTypeService CreateContractTypeService(
        out Mock<IAsyncRepository<ContractType>> contractTypeRepository,
        out Mock<IAsyncRepository<Company>> companyRepository
        )
    {
        contractTypeRepository = new();
        companyRepository = new();

        var contractTypeService = new ContractTypeService(contractTypeRepository.Object, companyRepository.Object);

        return contractTypeService;
    }

    public static LanguageService CreateLanguageService(
        out Mock<IAsyncRepository<Language>> languageRepository,
        out Mock<IAsyncRepository<LanguageLevel>> languageLevelRepository)
    {
        languageRepository = new();
        languageLevelRepository = new();

        var languageService = new LanguageService(languageRepository.Object, languageLevelRepository.Object);

        return languageService;
    }

    public static PriorityLevelService CreatePriorityLevelService(out Mock<IAsyncRepository<PriorityLevel>> priorityLevelRepository,
        Mock<IAsyncRepository<PriorityLevelTranslation>> priorityLevelTranslationRepository = null)
    {
        priorityLevelRepository = new();
        priorityLevelTranslationRepository ??= new Mock<IAsyncRepository<PriorityLevelTranslation>>();

        var priorityLevelService = new PriorityLevelService(priorityLevelRepository.Object,
            priorityLevelTranslationRepository.Object);

        return priorityLevelService;
    }

    public static SeniorityService CreateSeniorityService(out Mock<IAsyncRepository<Seniority>> seniorityRepository)
    {
        seniorityRepository = new();

        var seniorityService = new SeniorityService(seniorityRepository.Object);

        return seniorityService;
    }

    public static SkillService CreateSkillService(out Mock<IAsyncRepository<Skill>> skillRepository)
    {
        skillRepository = new();

        var skillService = new SkillService(skillRepository.Object);

        return skillService;
    }

    public static PolicyService CreatePolicyService(
        out Mock<IAsyncRepository<Policy>> policyRepository,
        out Mock<IAsyncRepository<Company>> companyRepository
        )
    {
        policyRepository = new();
        companyRepository = new();

        var policyService = new PolicyService(policyRepository.Object, companyRepository.Object);

        return policyService;
    }

    public static StoryPointService CreateStoryPointService(out Mock<IAsyncRepository<StoryPoint>> storyPointsRepository)
    {
        storyPointsRepository = new();

        var storyPointService = new StoryPointService(storyPointsRepository.Object);

        return storyPointService;
    }

    public static UserSettingsService CreateUserSettingsService(
        out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
        out Mock<IAsyncRepository<UserSettings>> userSettingsRepository,
        out Mock<UserManager<User>> userManager,
        Mock<IAsyncRepository<Language>> languageRepository = null
        )
    {
        var userStoreMock = new Mock<IUserStore<User>>();

        userManager = new Mock<UserManager<User>>(
            userStoreMock.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object
            );

        timeZoneRepository = new();
        userSettingsRepository = new();
        languageRepository ??= new Mock<IAsyncRepository<Language>>();

        var userSettingsService = new UserSettingsService(
            timeZoneRepository.Object,
            userSettingsRepository.Object,
            userManager.Object,
            languageRepository.Object
            );

        return userSettingsService;
    }

    public static CompanyUserInvitationService CreateCompanyUserInvitationService(
        out Mock<IAsyncRepository<CompanyUserInvitation>> companyUserInvitationRepository,
        out Mock<IAsyncRepository<CompanyUser>> companyUserRepository,
        out Mock<IAsyncRepository<Company>> companyRepository,
        out Mock<IAsyncRepository<JobRole>> jobRoleRepository,
        out Mock<RoleManager<Role>> roleManager,
        out Mock<IAsyncRepository<Project>> projectRepository,
        Mock<ISendGridService> sendGridService)
    {
        var roleStoreMock = new Mock<IRoleStore<Role>>();

        roleManager = new Mock<RoleManager<Role>>(
           roleStoreMock.Object,
           new IRoleValidator<Role>[0],
           new Mock<ILookupNormalizer>().Object,
           new Mock<IdentityErrorDescriber>().Object,
           new Mock<ILogger<RoleManager<Role>>>().Object
           );

        companyUserInvitationRepository = new();
        companyRepository = new();
        companyUserRepository = new();
        jobRoleRepository = new();
        projectRepository = new();
        sendGridService ??= new Mock<ISendGridService>();

        var companyUserInvitationService = new CompanyUserInvitationService(
            companyUserInvitationRepository.Object,
            companyUserRepository.Object,
            companyRepository.Object,
            roleManager.Object,
            projectRepository.Object,
            sendGridService.Object);

        return companyUserInvitationService;
    }
}