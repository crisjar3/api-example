using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Queries.DailyTimeBlock;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.User;

public static class FindSumaryOfUsersList
{
    public static IQueryBuilder FindSummaryUsersList(this IQueryBuilder builder, IEnumerable<int> userIds, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, int companyId, double timeZone)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
            .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
            .WithClose()
            .Select(" count( distinct cu.id) as TotalUsers,\n " +
           " CONCAT(CAST(FLOOR(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND) / 3600)) AS STRING),'h ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)),3600) / 60) AS STRING),'m ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)), 60))  AS STRING) , 's') AS TimeWork,\n" +
           " COUNT(DISTINCT t.goal_id) AS TotalGoals\n ")
           .From(" public.project p\n ")
           .Join(" public.project_company_user pcu ", " pcu.project_id = p.id\n ")
           .Join(" public.company_user cu ", " cu.id = pcu.company_user_id\n ")
           .Join(" public.task t ", " t.owner_id  = pcu.id\n ")
           .Join(" daily_time_block_adjusted dtb ", " dtb.task_id  = t.id\n ")
           .Where($" p.company_id = {companyId}\n ")
           .Where($" dtb.dateday >= DATETIME '{startDate.ToString("yyyy-MM-dd HH:mm:ss")}'\n")
           .Where($" dtb.dateday <= DATETIME '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}'\n");

        //verify if seach for projectId
        if (!projectIds.IsNullOrEmpty())
            builder.Where($" p.id ").In(projectIds);

        if (!userIds.IsNullOrEmpty())
            builder.Where($" cu.id ").In(userIds);

        return builder;
    }
}
