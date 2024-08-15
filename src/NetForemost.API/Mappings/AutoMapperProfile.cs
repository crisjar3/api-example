using AutoMapper;
using NetForemost.API.Requests.Account;
using NetForemost.API.Requests.Authentication;
using NetForemost.API.Requests.Benefits;
using NetForemost.API.Requests.Companies;
using NetForemost.API.Requests.Goals;
using NetForemost.API.Requests.JobOffers;
using NetForemost.API.Requests.JobRoles;
using NetForemost.API.Requests.Projects;
using NetForemost.API.Requests.Tasks;
using NetForemost.API.Requests.Timers;
using NetForemost.Core.Dtos.Account;
using NetForemost.Core.Dtos.AppClients;
using NetForemost.Core.Dtos.Benefits;
using NetForemost.Core.Dtos.Cities;
using NetForemost.Core.Dtos.Companies;
using NetForemost.Core.Dtos.Companies.CompanyUserInvitations;
using NetForemost.Core.Dtos.ContractTypes;
using NetForemost.Core.Dtos.Countries;
using NetForemost.Core.Dtos.Goals;
using NetForemost.Core.Dtos.Industries;
using NetForemost.Core.Dtos.JobOffers;
using NetForemost.Core.Dtos.JobRoles;
using NetForemost.Core.Dtos.Languages;
using NetForemost.Core.Dtos.Policies;
using NetForemost.Core.Dtos.PriorityLevels;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Dtos.Seniorities;
using NetForemost.Core.Dtos.Skills;
using NetForemost.Core.Dtos.StoryPoints;
using NetForemost.Core.Dtos.Tasks;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Dtos.TimeZone;
using NetForemost.Core.Entities.AppClients;
using NetForemost.Core.Entities.Benefits;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.ContractTypes;
using NetForemost.Core.Entities.Countries;
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
using NetForemost.Core.Entities.TimeZones;
using NetForemost.Core.Entities.Users;
using NetForemost.Infrastructure.Importer.Entities;
using NetForemost.SharedKernel.Entities;
using System.Linq;

namespace NetForemost.API.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //User
        CreateMap<PaginatedRecord<User>, PaginatedRecord<UserDto>>().ReverseMap();

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Companies, opt => opt.MapFrom(dest => dest.CompanyUsers.Select(user => user)))
            .ReverseMap();

        CreateMap<CompanyUser, CompanyRoleDto>()
            .ForMember(dest => dest.company, opt => opt.MapFrom(dest => dest.Company))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(dest => dest.Role)).
            ReverseMap();

        CreateMap<User, PutUserRequest>().ReverseMap();

        CreateMap<User, PostUserRequest>().ReverseMap();
        CreateMap<User, PutImageUserRequest>().ReverseMap();
        CreateMap<User, PostTalentUserRequest>().ReverseMap();
        CreateMap<PostUserLanguageRequest, UserLanguage>().ReverseMap();

        CreateMap<UserSettings, UserSettingsDto>().ReverseMap();
        CreateMap<UserSettings, PostUserSettingsRequest>().ReverseMap();
        CreateMap<UserSettings, PutUserSettingsRequest>().ReverseMap();

        //Company
        CreateMap<Company, PostCompanyRequest>().ReverseMap();
        CreateMap<Company, PutCompanyRequest>().ReverseMap();
        CreateMap<Company, CompanyDto>().ReverseMap()
        .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone));
        CreateMap<Company, PatchCompanyDetailsDto>().ReverseMap();

        //Company Settings
        CreateMap<CompanySettings, PostCompanySettingsRequest>().ReverseMap();
        CreateMap<CompanySettings, PutCompanySettingsRequest>().ReverseMap();
        CreateMap<CompanySettings, CompanySettingsDto>().ReverseMap();
        CreateMap<CompanyUser, UserSettingCompanyUserDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => src.JobRole))
            .ReverseMap();
        CreateMap<PaginatedRecord<CompanyUser>, PaginatedRecord<UserSettingCompanyUserDto>>().ReverseMap();


        //Company User
        CreateMap<PutTeamMembersRequest, CompanyUser>().ReverseMap();
        CreateMap<PutTeamMembersRequest, User>().ReverseMap();
        CreateMap<CompanyUser, CompanyUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => src.JobRole))
            .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone));

        CreateMap<PostTeamMemberRequest, CompanyUser>().ReverseMap();
        CreateMap<PaginatedRecord<CompanyUser>, PaginatedRecord<CompanyUserDto>>().ReverseMap();
        CreateMap<CompanyUser, SimpleCompanyUserDto>()
            .ReverseMap();

        CreateMap<PaginatedRecord<CompanyUser>, PaginatedRecord<SimpleCompanyUserDto>>().ReverseMap();

        CreateMap<CompanyUser, CompanyUserSettingsDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.ProjectsCompanyUser, opt => opt.MapFrom(src => src.ProjectCompanyUsers))
            .ReverseMap();

        CreateMap<CompanyUser, PutCompanyUserUserSettingsDto>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
            .ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => src.JobRole))
            .ReverseMap();

        CreateMap<PostCompanyUserInvitationRequest, CompanyUserInvitation>().ReverseMap();
        CreateMap<CompanyUserInvitation, CompanyUserInvitationDto>().ReverseMap();

        //City
        CreateMap<City, CityDto>().ReverseMap();

        //Country
        CreateMap<Country, CountryDto>().ReverseMap();

        //Time Zone
        CreateMap<TimeZone, TimeZoneDto>().ReverseMap();

        //Job Role
        CreateMap<JobRole, JobRoleDto>().ReverseMap();
        CreateMap<JobRoleCategory, PostCustomJobRoleCategoryRequest>().ReverseMap();
        CreateMap<JobRole, PostCustomJobRoleRequest>().ReverseMap();

        CreateMap<JobRoleCategory, JobRoleCategoryDto>()
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(dest => dest.JobRoleCategorySkills.Select(jobRoleCategorySkill => jobRoleCategorySkill.Skill)))
            .ReverseMap();
        CreateMap<JobRole, JobRoleDetailsDto>().ReverseMap();

        //Role
        CreateMap<Role, RoleDto>().ReverseMap();

        //Benefit
        CreateMap<Benefit, BenefitDto>().ReverseMap();
        CreateMap<Benefit, PostBenefitCompanyRequest>().ReverseMap();

        //Skill
        CreateMap<Skill, SkillDto>().ReverseMap();

        //UserSkill
        CreateMap<UserSkill, PostUserSkillRequest>().ReverseMap();
        CreateMap<UserSkill, UserSkillDto>().ReverseMap();

        //Seniority
        CreateMap<Seniority, SeniorityDto>().ReverseMap();

        //Team
        CreateMap<JobOffer, PostJobOfferRequest>().ReverseMap();
        CreateMap<JobOfferBenefit, JobOfferBenefitRequest>().ReverseMap();
        CreateMap<JobOfferTalent, JobOfferTalentRequest>().ReverseMap();
        CreateMap<JobOfferTalentSkill, JobOfferTalentSkillRequest>().ReverseMap();

        CreateMap<JobOffer, JobOfferDto>().ReverseMap();
        CreateMap<JobOfferBenefit, JobOfferBenefitDto>().ReverseMap();
        CreateMap<JobOfferTalent, JobOfferTalentDto>().ReverseMap();
        CreateMap<JobOfferTalentSkill, JobOfferTalentSkillDto>().ReverseMap();

        //AppClient
        CreateMap<AppClient, AppClientDto>().ReverseMap();
        CreateMap<AppClient, PostAppClientRequest>().ReverseMap();

        //Project
        CreateMap<Project, ProjectDto>()
            .ForMember(s => s.UserProjects, opt => opt.MapFrom(ss => ss.ProjectCompanyUsers.Select(bs => bs.CompanyUser.User)))
            .ForMember(s => s.ProjectImageUrl, opt => opt.MapFrom(ss => ss.ProjectImageUrl))
            .ReverseMap();

        CreateMap<Project, ProjectDto>()
            .ForMember(s => s.ProjectCompanyUsers, opt => opt.MapFrom(ss => ss.ProjectCompanyUsers.Select(bs => bs.CompanyUser.User)))
            .ForMember(s => s.ProjectImageUrl, opt => opt.MapFrom(ss => ss.ProjectImageUrl))
            .ReverseMap();

        CreateMap<PaginatedRecord<Project>, PaginatedRecord<ProjectDto>>().ReverseMap();
        CreateMap<PostProjectRequest, Project>().ReverseMap();
        CreateMap<PutProjectRequest, Project>().ReverseMap();
        CreateMap<GetProjectRequest, Project>().ReverseMap();

        CreateMap<PostProjectCompanyUserRequest, ProjectCompanyUser>().ReverseMap();
        CreateMap<ProjectCompanyUser, ProjectCompanyUserDto>().ReverseMap();
        CreateMap<ProjectCompanyUser, ProjectCompanyUserSettingsDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
            .ReverseMap();
        CreateMap<Project, ProjectSettingDto>().ReverseMap();
        CreateMap<Project, PostProjectDto>()
            .ForMember(s => s.ProjectImageUrl, opt => opt.MapFrom(ss => ss.ProjectImageUrl))
            .ReverseMap();
        CreateMap<Project, ProjectStatusDto>()
            .ForMember(s => s.ProjectImageUrl, opt => opt.MapFrom(ss => ss.ProjectImageUrl))
            .ReverseMap();
        CreateMap<PaginatedRecord<Project>, PaginatedRecord<ProjectStatusDto>>().ReverseMap();
        CreateMap<ProjectCompanyUser, ProjectCompanyUserStatusDto>()
            .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(project => project.Project.Id))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(project => project.Project.Name))
            .ReverseMap();
        CreateMap<Project, ProjectProjectAvatarDto>()
            .ForMember(s => s.ProjectImageUrl, opt => opt.MapFrom(ss => ss.ProjectImageUrl))
            .ReverseMap();
        CreateMap<ProjectAvatar, ProjectAvatarDto>().ReverseMap();


        //Authentication Role
        CreateMap<Role, RoleDto>().ReverseMap();

        //Project Company User
        CreateMap<ProjectCompanyUser, ProjectCompanyUserDto>().
            ForMember(dest => dest.Project, opt => opt.MapFrom(src => new ProjectDto())).
            ForMember(dest => dest.JobRole, opt => opt.MapFrom(src => new JobRoleDto())).ReverseMap();

        //Industry
        CreateMap<Industry, IndustryDto>().ReverseMap();

        //Policy
        CreateMap<PolicyDto, Policy>().ReverseMap();

        //Goals
        CreateMap<PaginatedRecord<Goal>, PaginatedRecord<GoalDto>>().ReverseMap();
        CreateMap<PaginatedRecord<GoalExtraMile>, PaginatedRecord<GoalExtraMileResumeDto>>().ReverseMap();
        CreateMap<Goal, GoalDto>().ReverseMap();
        CreateMap<Goal, GoalResumeDto>().ReverseMap();
        CreateMap<PostGoalRequest, Goal>().ReverseMap();
        CreateMap<PostExtraMileGoalRequest, GoalExtraMile>().ReverseMap();
        CreateMap<GoalExtraMile, GoalExtraMileDto>().ReverseMap();
        CreateMap<GoalExtraMile, GoalExtraMileResumeDto>().ReverseMap();
        CreateMap<PutGoalRequest, Goal>().ReverseMap();

        CreateMap<FindAllGoalsDto, Goal>().ReverseMap();

        CreateMap<GetStoryPointDto, StoryPoint>().ReverseMap();
        CreateMap<GetPriorityLevelDto, PriorityLevel>().ReverseMap();
        CreateMap<GetProjectDto, Project>().ReverseMap();
        CreateMap<GetGoalStatusDto, GoalStatus>().ReverseMap();
        CreateMap<GetUserDto, User>().ReverseMap();


        // GoalStatus
        CreateMap<GoalStatus, GoalStatusDto>().ReverseMap();
        CreateMap<GoalStatusCategory, GoalStatusCategoryDto>().ReverseMap();

        //Languages
        CreateMap<Language, LanguageDto>().ReverseMap();
        CreateMap<LanguageLevel, LanguageLevelDto>().ReverseMap();
        CreateMap<UserLanguage, UserLanguageDto>().ReverseMap();
        //Contract type
        CreateMap<ContractTypeDto, ContractType>().ReverseMap();

        //Priority Levels
        CreateMap<PriorityLevel, PriorityLevelDto>().ReverseMap();

        //Story Points
        CreateMap<StoryPoint, StoryPointDto>().ReverseMap();

        //Tasks
        CreateMap<PutTaskTypeRequest, TaskType>().ReverseMap();
        CreateMap<PostTaskRequest, Task>().ReverseMap();
        CreateMap<PutTaskRequest, Task>().ReverseMap();
        CreateMap<PostTaskTypeRequest, TaskType>().ReverseMap();
        CreateMap<TaskType, TaskTypeDto>().ReverseMap();
        CreateMap<Task, TaskDto>().ReverseMap();
        CreateMap<PaginatedRecord<Task>, PaginatedRecord<TaskDto>>().ReverseMap();
        CreateMap<PaginatedRecord<TaskType>, PaginatedRecord<TaskTypeDto>>().ReverseMap();

        CreateMap<Task, GetTasksQueryableDto>().ReverseMap();
        CreateMap<TaskType, GetTypeTaskDto>().ReverseMap();

        //Tracking
        CreateMap<DailyTimeBlock, DailyTimeBlockDto>().ReverseMap();
        CreateMap<PostDailyTimeBlockRequest, DailyTimeBlock>().ReverseMap();

        CreateMap<PostDailyMonitoringBlockRequest, DailyMonitoringBlock>().ReverseMap();
        CreateMap<DailyMonitoringBlockDto, DailyMonitoringBlock>().ReverseMap();
        #region Importer Maps
        CreateMap<Benefit, BenefitImporter>().ReverseMap();

        CreateMap<JobRoleCategory, JobRoleCategoryImporter>().ReverseMap();

        CreateMap<JobRole, JobRoleImporter>().ReverseMap();

        CreateMap<JobRoleCategorySkillImporter, JobRoleCategorySkill>().ReverseMap();

        CreateMap<ContractTypeImporter, ContractType>().ReverseMap();

        CreateMap<PolicyImporter, Policy>().ReverseMap();

        CreateMap<GoalStatusCategoryImporter, GoalStatusCategory>().ReverseMap();
        #endregion
    }
}