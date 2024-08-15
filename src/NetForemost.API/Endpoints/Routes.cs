namespace NetForemost.API.Endpoints;

public static class Routes
{
    private const string ApiRoot = "v{version:apiVersion}";

    public static class Image
    {
        public const string PostImage = ApiRoot + "/images";
    }
    public static class Account
    {
        public const string PutUser = ApiRoot + "/account/users";
        public const string GetUser = ApiRoot + "/account/users/{id}";
        public const string GetUsersByRole = ApiRoot + "/account/users/role/{RoleName}";

        public const string PostUser = ApiRoot + "/account/users";
        public const string PostTalentUser = ApiRoot + "/account/talent-users";

        public const string PostUpdatePassword = ApiRoot + "/account/update-password";
        public const string PostConfirmEmail = ApiRoot + "/account/confirm-email";
        public const string PostConfirmEmailToken = ApiRoot + "/account/confirm-email-token";
        public const string PostResetPassword = ApiRoot + "/account/reset-password";
        public const string PostResetPasswordToken = ApiRoot + "/account/reset-password-token";


        public const string PostUserSettings = ApiRoot + "/account/settings";
        public const string PutUserSettings = ApiRoot + "/account/settings";
        public const string PutImageUser = ApiRoot + "/account/image-user";
        public const string GetVerifyUser = ApiRoot + "/account/verify-user";
        public const string GetUserSettings = ApiRoot + "/account/settings/{Id}";

        public const string GetRoles = ApiRoot + "/account/roles";
    }

    public static class Authentication
    {
        public const string PostLogin = ApiRoot + "/auth/login";
        public const string PostRefreshToken = ApiRoot + "/auth/refresh-token";
        public const string PostLogout = ApiRoot + "/auth/logout";
        public const string PostAppClient = ApiRoot + "/auth/app-client";
    }

    public static class Company
    {
        public const string PostCompany = ApiRoot + "/companies";
        public const string PutCompany = ApiRoot + "/companies";
        public const string PutImageCompany = ApiRoot + "/image_company";
        public const string GetCompany = ApiRoot + "/companies";
        public const string GetCompanyUsers = ApiRoot + "/company-users";
        public const string GetCompanyArchivedUsers = ApiRoot + "/company-users/archived";

        public const string PostCompanySettings = ApiRoot + "/companies/settings";
        public const string PutCompanySettings = ApiRoot + "/companies/settings";
        public const string GetCompanySettings = ApiRoot + "/companies/settings";

        public const string PostTeamMemberInvitation = ApiRoot + "/companies/team/member/invitation";
        public const string GetTeamMemberInvitation = ApiRoot + "/companies/team/member/invitation";
        public const string PostTeamMember = ApiRoot + "/companies/team/members";
        public const string GetTeamMember = ApiRoot + "/companies/team/members";
        public const string GetCompanyUserDetails = ApiRoot + "/companies/company-user/{id}";
        public const string PutTeamMember = ApiRoot + "/companies/team/members";
        public const string PatchTeamMember = ApiRoot + "/companies/team/members";

        public const string DeleteCompanyUser = ApiRoot + "/companies/team/members";
        public const string PatchCompanyDetails = ApiRoot + "/companies/patch-details";
        public const string PatchCompanySettings = ApiRoot + "/companies/patch-settings";
        public const string PostCompanyUserArchive = ApiRoot + "/companies/archive-user";
        public const string PostCompanyUserUnarchive = ApiRoot + "/companies/unarchive-user";
    }

    public static class TimeZone
    {
        public const string GetTimeZones = ApiRoot + "/time-zones";
    }

    public static class JobRole
    {
        public const string GetJobRoles = ApiRoot + "/job-roles";
        public const string GetJobRoleCategories = ApiRoot + "/job-roles/categories";
        public const string PostCustomJobRoleCategory = ApiRoot + "/job-roles/categories/custom-categories";
        public const string PostCustomJobRole = ApiRoot + "/job-roles/custom-job-roles";
    }

    public static class Benefit
    {
        public const string GetBenefits = ApiRoot + "/benefits/{CompanyId}";
        public const string PostBenefitCompany = ApiRoot + "/benefits/custom-benefit";
    }

    public static class Skill
    {
        public const string GetSkills = ApiRoot + "/skills";
    }

    public static class JobOffer
    {
        public const string PostJobOffer = ApiRoot + "/job-offer";
        public const string PutJobOfferStatus = ApiRoot + "/job-offer/status";
        public const string GetJobOffer = ApiRoot + "/job-offer";
    }

    public static class Country
    {
        public const string GetCountries = ApiRoot + "/countries";
    }

    public static class City
    {
        public const string GetCities = ApiRoot + "/cities/{CountryId}";
    }

    public static class Project
    {
        public const string GetProjectById = ApiRoot + "/projects/{ProjectId}/{CompanyId}";
        public const string GetProjects = ApiRoot + "/projects";
        public const string PostProject = ApiRoot + "/projects";
        public const string PutProject = ApiRoot + "/projects";
        public const string DeleteProject = ApiRoot + "/projects";
        public const string GetProjectsByCompanyId = ApiRoot + "/projects/company";
        public const string GetUsersByProjectCompany = ApiRoot + "/projects/company-users";

        public const string PostProjectCompanyUser = ApiRoot + "/projects/company-users";
        public const string PostProjectCompanyUserListByCompanyUsersIds = ApiRoot + "/projects/company-users/company-user-id-list";
        public const string PutProjectCompanyUserListByCompanyUsersIds = ApiRoot + "/projects/company-users/company-user-id-list";
        public const string PutProjectCompanyUserStatus = ApiRoot + "/projects/company-users/status";
        public const string PutProjectCompanyUsersActiveStatusList = ApiRoot + "/projects/company-users/update-active-status";
        public const string PostProjectCompanyUserList = ApiRoot + "/projects/company-users/list";
        public const string PostProjectAvatar = ApiRoot + "/projects/project-avatar";
        public const string GetProjectAvatar = ApiRoot + "/projects/project-avatars";
        public const string PostProjectArchive = ApiRoot + "/projects/archive";
        public const string PostProjectUnarchive = ApiRoot + "/projects/unarchive";
        public const string PatchProject = ApiRoot + "/projects";
    }

    public static class Dashboard
    {
        public const string GetDashboard = ApiRoot + "/dashboard";
    }

    public static class Seniority
    {
        public const string GetSeniorities = ApiRoot + "/seniorities";
    }

    public static class Industry
    {
        public const string GetIndustry = ApiRoot + "/industries";
    }

    public static class Goals
    {
        public const string Root = ApiRoot + "/goals";
        public const string PutConfirmGoal = Root + "/confirm-goal";
        public const string ExtraMileRoot = ApiRoot + "/goals/extra-mile";
        public const string GetActiveGoalsSummary = ApiRoot + "/goals/active/summary";
        public const string GetActiveGoals = ApiRoot + "/goals/active";
        public const string GetActiveExtraMileGoals = ApiRoot + "/goals/active/extra-mile";
        public const string GetAllExtraMileGoals = ApiRoot + "/goals/extra-miles";
        public const string PutGoalStatus = ApiRoot + "/goals/{goalId}/status";
        public const string PutGoal = Root;
        public const string GetLateGoals = Root + "/late";
        public const string GetCompletedGoals = Root + "/completed/on-time";

    }

    public static class GoalStatus
    {
        public const string Root = ApiRoot + "/goalStatuses";
    }

    public static class Policy
    {
        public const string GetPolicies = ApiRoot + "/policies";
    }


    public static class Language
    {
        public const string GetLanguages = ApiRoot + "/languages";
        public const string GetLanguageLevels = ApiRoot + "/languages/levels";
    }

    public static class ContractType
    {
        public const string GetContractTypes = ApiRoot + "/contract-types";
    }

    public static class Talent
    {
        public const string GetTalents = ApiRoot + "/talents";
    }

    public static class PriorityLevels
    {
        public const string GetPriorityLevels = ApiRoot + "/priority-levels";
    }

    public static class StoryPoints
    {
        public const string Root = ApiRoot + "/story-points";
    }

    public static class Tasks
    {
        public const string Root = ApiRoot + "/tasks";
        public const string TaskTypes = Root + "/types";
        public const string GetTaskById = Root + "/{id}";
        public const string PostTaskTypesActivate = Root + "/types-activate";
        public const string PostTaskTypesDeactivate = Root + "/types-deactivate";
        public const string GetTaskTypesRecently = Root + "/types/recently-by-companyUser";
    }

    public static class Timer
    {
        public const string Root = ApiRoot + "/timer";
        public const string PostDailyEntry = Root + "/daily-entry";
        public const string PostDailyTimeBlock = Root + "/daily-time-block";
        public const string PostDailyMonitoringBlock = Root + "/daily-monitoring-block";

        public const string GetDailyEntryByDay = Root + "/daily-entry";
    }
}