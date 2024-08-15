namespace NetForemost.Report.API.Endpoints;
public static class Routes
{
    private const string ApiRoot = "v{version:apiVersion}";
    public static class Timer
    {
        //Timer Route
        public const string Root = ApiRoot + "/timer";
        public const string GetBlockTimes = Root;
        public const string GetMonitoringsByBlock = Root + "/monitoring-block";
        public const string GetAllDailyEntriesByDateRangesPerUser = Root + "/daily-entries";
        public const string GetTimeBlocksExport = Root + "/export-summary";
        public const string GetTimeBlocksForCopyPaste = Root + "/copy";
    }

    public static class UserTimeLine
    {
        //UsersTimeLine Route
        public const string GetUsers = ApiRoot + "/users-time-Line";
        public const string GetSummary = ApiRoot + "/summary-timeline";
        public const string GetAllUsers = ApiRoot + "/users";
        public const string GetExportUserSummary = ApiRoot + "/users/export-summary";
    }

    public static class Goal
    {
        public const string Root = ApiRoot + "/goals";
        public const string GetGoals = Root + "/goals-by-project";
        public const string GetSummary = ApiRoot + "/summary-timeline";
    }

    public static class ProjectAndGoal
    {
        //ProjectAndGoals Rout
        public const string GetProjectSummary = ApiRoot + "/project";
        public const string GetGoalData = ApiRoot + "/goal";
        public const string GetProjectsByCompany = ApiRoot + "/projects-by-company";
        public const string GetProjectAndGoalsSummary = ApiRoot + "/summary";
        public const string GetExportProjectSummary = ApiRoot + "/project/export-summary";
    }

    public static class TimeZone
    {
        public const string GetAllTimeZones = ApiRoot + "/timezone";
        public const string GetProjectAndGoalsSummary = ApiRoot + "/summary";
    }
}