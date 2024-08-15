using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Queries.DailyTimeBlock;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.User;

public static class GetUserSummary
{
    public static IQueryBuilder GetUsersSummary(this IQueryBuilder builder, IEnumerable<int> userIds, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone, int companyId)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
                .Select(" anu.user_image_url as ImageUrl, cu.id as UserId,  anu.user_name as UserName, ro.name AS Role, " +
                " CONCAT(CAST(FLOOR(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND) / 3600)) AS STRING),'h ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)),3600) / 60) AS STRING),'m ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)), 60))  AS STRING) , 's') AS TimeWorked, " +
                " COUNT(DISTINCT t.goal_id) AS GoalWorked ")
                .From(" daily_time_block_adjusted dtb ")
                .Join(" public.task t ", " t.id = dtb.task_id ")
                .Join(" public.project_company_user pcu ", "pcu.id = t.owner_id")
                .Join(" public.company_user cu ", " cu.id = pcu.company_user_id")
                .Join(" public.AspNetUsers anu ", " anu.id = cu.user_id ")
                .Join(" public.job_role ro", " anu.job_role_id = ro.id ", " LEFT ");

        if (!projectIds.IsNullOrEmpty())
            builder.Join(" public.project p ", " p.id = t.project_id ");

        builder.Where($" cu.company_id = {companyId}")
            .Where($" dtb.dateday >= DATETIME '{startDate.ToString("yyyy-MM-dd HH:mm:ss")}'")
            .Where($" dtb.dateday <= DATETIME '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}'");

        //verify if seach for projectId
        if (!projectIds.IsNullOrEmpty())
            builder.Where($" p.id ").In(projectIds);

        if (!userIds.IsNullOrEmpty())
            builder.Where($" cu.id ").In(userIds);

        //Add groupsby to query
        builder.GroupBy(" cu.id, anu.user_name , anu.user_image_url , ro.name ");

        return builder;
    }
    public static IQueryBuilder GetUsersSummaryExport(this IQueryBuilder builder, IEnumerable<int> userIds, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone, int companyId)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
                .Select(" cu.id as UserId, anu.user_name as UserName, " +
                " CONCAT(CAST(FLOOR(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND) / 3600)) AS STRING),'h ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)),3600) / 60) AS STRING),'m ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)), 60))  AS STRING) , 's') AS TimeWorked, " +
                " COUNT(DISTINCT t.goal_id) AS GoalWorked ")
                .From(" daily_time_block_adjusted dtb ")
                .Join(" public.task t ", " t.id = dtb.task_id ")
                .Join(" public.project_company_user pcu ", "pcu.id = t.owner_id")
                .Join(" public.company_user cu ", " cu.id = pcu.company_user_id")
                .Join(" public.AspNetUsers anu ", " anu.id = cu.user_id ");

        if (!projectIds.IsNullOrEmpty())
            builder.Join(" public.project p ", " p.id = t.project_id ");

        builder.Where($" cu.company_id = {companyId}")
            .Where($" dtb.dateday >= DATETIME '{startDate.ToString("yyyy-MM-dd HH:mm:ss")}'")
            .Where($" dtb.dateday <= DATETIME '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}'");

        //verify if seach for projectId
        if (!projectIds.IsNullOrEmpty())
            builder.Where($" p.id ").In(projectIds);

        if (!userIds.IsNullOrEmpty())
            builder.Where($" cu.id ").In(userIds);

        //Add groupsby to query
        builder.GroupBy(" cu.id, anu.user_name ");

        return builder;
    }
    public static IQueryBuilder GetUsersSummaryPaginated(this IQueryBuilder builder, int perPage, int pageNumber)
    {
        builder.Limit(perPage).Offset(pageNumber, perPage);

        return builder;
    }
}
