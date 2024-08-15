using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Queries.DailyTimeBlock;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.Project;

public static class FindProjectsSummary
{
    public static IQueryBuilder ProjectsSummary(this IQueryBuilder builder, IEnumerable<int> CompanyUserIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
                .Select(" count( distinct p.id) as TotalProjects, " +
                " CONCAT(CAST(FLOOR(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND) / 3600)) AS STRING),'h ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)),3600) / 60) AS STRING),'m ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)), 60))  AS STRING) , 's') AS TimeWork, " +
                " COUNT(DISTINCT t.goal_id) AS TotalGoals ")
                .From(" public.project p ")
                .Join(" public.project_company_user pcu ", " pcu.project_id = p.id");

        if (!CompanyUserIds.IsNullOrEmpty())
            builder.Join(" public.company_user cu ", " cu.id = pcu.company_user_id");

        builder.Join(" public.task t ", " t.owner_id  = pcu.id ")
             .Join(" daily_time_block_adjusted dtb ", " dtb.task_id  = t.id ")
             .Where($" p.company_id = {companyId}")
             .Where($" dtb.DateDay >= DateTime'{startDate.ToString("yyyy-MM-dd HH:mm:ss")}'")
             .Where($" dtb.DateDay <= DateTime'{endDate.ToString("yyyy-MM-dd HH:mm:ss")}'");

        //verify if seach for projectId
        if (!projectIds.IsNullOrEmpty())
            builder.Where($" p.id ").In(projectIds);

        if (!CompanyUserIds.IsNullOrEmpty())
            builder.Where($" cu.id ").In(CompanyUserIds);

        return builder;
    }
}

