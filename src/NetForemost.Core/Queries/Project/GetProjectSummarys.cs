using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Queries.DailyTimeBlock;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.Project;

public static class GetProjectSummarys
{
    public static IQueryBuilder GetProjectsSummary(this IQueryBuilder builder, IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
                .Select(" \n p.project_image_url as ImageUrl , p.id AS ProjectId, p.name AS ProjectName ,  " +
               " \nCONCAT(CAST(FLOOR(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND) / 3600)) AS STRING),'h ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)),3600) / 60) AS STRING),'m ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)), 60))  AS STRING) ,'s') AS TimeWorked, " +
               " \nCOUNT(DISTINCT t.goal_id) AS GoalsWorked, COUNT(DISTINCT dtb.owner_id) AS UsersWorked ")
               .From(" \npublic.project p ")
               .Join(" \npublic.project_company_user pcu ", " p.id = pcu.project_id ");

        if (!userIds.IsNullOrEmpty())
            builder.Join(" \npublic.company_user cu", " cu.id = pcu.company_user_id");

        builder.Join(" \npublic.task t ", " t.owner_id = pcu.id")
                .Join(" \ndaily_time_block_adjusted dtb ", " dtb.task_id = t.id ")
                .Where($" \np.company_id = {companyId}")
                .Where($" \ndtb.dateday >= DATETIME '{startDate.ToString("yyyy-MM-dd HH:mm:ss")}'")
                .Where($" \ndtb.dateday <= DATETIME '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}'");

        //verify if seach for projectId
        if (!projectIds.IsNullOrEmpty())
            builder.Where($" p.id ").In(projectIds);

        if (!userIds.IsNullOrEmpty())
            builder.Where($" cu.id ").In(userIds);

        //Add group by to query
        builder.GroupBy(" p.id, p.name , p.project_image_url ");

        return builder;
    }

    public static IQueryBuilder GetProjectsSummaryExport(this IQueryBuilder builder, IEnumerable<int> userIds, int companyId, IEnumerable<int> projectIds, DateTime startDate, DateTime endDate, double timeZone)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
                .Select(" \np.id AS ProjectId, p.name AS ProjectName ,  " +
               " \nCONCAT(CAST(FLOOR(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND) / 3600)) AS STRING),'h ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)),3600) / 60) AS STRING),'m ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)), 60))  AS STRING) ,'s') AS TimeWorked, " +
               " \nCOUNT(DISTINCT t.goal_id) AS GoalsWorked, COUNT(DISTINCT dtb.owner_id) AS UsersWorked ")
               .From(" \npublic.project p ")
               .Join(" \npublic.project_company_user pcu ", " p.id = pcu.project_id ");

        if (!userIds.IsNullOrEmpty())
            builder.Join(" \npublic.company_user cu", " cu.id = pcu.company_user_id");

        builder.Join(" \npublic.task t ", " t.owner_id = pcu.id")
            .Join(" \ndaily_time_block_adjusted dtb ", " dtb.task_id = t.id ")
            .Where($" \np.company_id = {companyId}")
            .Where($" \ndtb.dateday >= DATETIME '{startDate.ToString("yyyy-MM-dd HH:mm:ss")}'")
            .Where($" \ndtb.dateday <= DATETIME '{endDate.ToString("yyyy-MM-dd HH:mm:ss")}'");

        //verify if seach for projectId
        if (!projectIds.IsNullOrEmpty())
            builder.Where($" p.id ").In(projectIds);

        if (!userIds.IsNullOrEmpty())
            builder.Where($" cu.id ").In(userIds);

        //Add group by to query
        builder.GroupBy(" p.id, p.name ");

        return builder;
    }
    public static IQueryBuilder GetProjectsSummaryPaginated(this IQueryBuilder builder, int perPage, int pageNumber)
    {
        // Add pagination
        builder.Limit(perPage).Offset(pageNumber, perPage);

        return builder;
    }
}
