using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetForemost.Core.Entities.AppClients;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Billings;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Groups;
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
using NetForemost.Infrastructure.Extensions;
using Task = NetForemost.Core.Entities.Tasks.Task;

namespace NetForemost.Infrastructure.Contexts;

public class NetForemostContext : IdentityDbContext<User, Role, string>
{
    public NetForemostContext()
    {
    }

    public NetForemostContext(DbContextOptions<NetForemostContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    //Billing
    public virtual DbSet<BillingHistory> BillingHistories { get; set; }
    public virtual DbSet<Feature> Features { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }
    public virtual DbSet<SubscriptionTypeFeature> SubscriptionTypeFeatures { get; set; }

    //Company
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<CompanySettings> CompanySettings { get; set; }
    public virtual DbSet<CompanyUser> CompanyUsers { get; set; }
    public virtual DbSet<CompanyUserInvitation> CompanyUserInvitations { get; set; }
    public virtual DbSet<ProjectCompanyUserInvitation> ProjectCompanyUserInvitations { get; set; }

    //Country 
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Country> Countries { get; set; }

    //Group
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<GroupUser> GroupUsers { get; set; }

    //Project
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<ProjectGroup> ProjectGroups { get; set; }
    public virtual DbSet<ProjectCompanyUser> ProjectCompanyUsers { get; set; }
    public virtual DbSet<ProjectAvatar> ProjectAvatars { get; set; }

    //Task
    public virtual DbSet<Task> WorkTasks { get; set; }
    public virtual DbSet<TaskType> TaskTypes { get; set; }


    //TimeTracking
    public virtual DbSet<Device> Devices { get; set; }
    public virtual DbSet<DeviceType> DeviceTypes { get; set; }
    public virtual DbSet<DailyTimeBlock> BlockTimeTrackings { get; set; }
    public virtual DbSet<DailyMonitoringBlock> TrackingBlockImages { get; set; }


    //User
    public virtual DbSet<UserSettings> UserSettings { get; set; }
    public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public virtual DbSet<UserSkill> UserSkills { get; set; }

    //TimeZone
    public virtual DbSet<Core.Entities.TimeZones.TimeZone> TimeZones { get; set; }

    //Language
    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<LanguageLevel> LanguageLevels { get; set; }

    //Roles
    public virtual DbSet<RoleTranslation> RoleTranslations { get; set; }

    //WorkRoles
    public virtual DbSet<JobRole> WorkRoles { get; set; }
    public virtual DbSet<JobRoleTranslation> WorkRoleTranslations { get; set; }
    public virtual DbSet<JobRoleCategory> WorkRoleCategories { get; set; }
    public virtual DbSet<JobRoleCategoryTranslation> WorkRoleCategoryTranslations { get; set; }
    public virtual DbSet<JobRoleCategorySkill> WorkRoleCategorySkills { get; set; }

    //Benefits
    public virtual DbSet<Benefit> Benefits { get; set; }

    //Skills
    public virtual DbSet<Skill> Skills { get; set; }

    //Seniorities
    public virtual DbSet<Seniority> Seniorities { get; set; }

    //Teammates
    public virtual DbSet<JobOffer> JobOffer { get; set; }
    public virtual DbSet<JobOfferBenefit> JobOfferBenefits { get; set; }
    public virtual DbSet<JobOfferTalent> JobOfferJobRoles { get; set; }
    public virtual DbSet<JobOfferTalentSkill> JobOfferJobRoleSkills { get; set; }

    //AppClient
    public virtual DbSet<AppClient> AppClients { get; set; }

    //Industry
    public virtual DbSet<Industry> Industries { get; set; }

    //Policy
    public virtual DbSet<Policy> Policies { get; set; }
    //Goals
    public virtual DbSet<Goal> Goals { get; set; }
    public virtual DbSet<GoalExtraMile> GoalsExtraMiles { get; set; }

    //Contract Type
    public virtual DbSet<ContractType> ContractTypes { get; set; }
    //Priority Levels
    public virtual DbSet<PriorityLevel> PriorityLevels { get; set; }
    public virtual DbSet<PriorityLevelTranslation> PriorityLevelTraslations { get; set; }
    //Story`points
    public virtual DbSet<StoryPoint> StoryPoints { get; set; }
    // Statuses
    public virtual DbSet<GoalStatusCategory> StatusCategories { get; set; }
    public virtual DbSet<GoalStatus> Statuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetSimpleUnderscoreTableNameConvention(true);
        modelBuilder.ApplyUtcDateTimeConverter();

        //modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<User>().Navigation(user => user.TimeZone).AutoInclude();
        modelBuilder.Entity<User>().Navigation(user => user.UserSettings).AutoInclude();
        modelBuilder.Entity<UserSettings>().Navigation(user => user.Language).AutoInclude();

        modelBuilder.Entity<CompanyUser>().HasQueryFilter(companyUser => companyUser.IsActive == true);
        base.OnModelCreating(modelBuilder);
    }
}