using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.DailyTimeBlock
{
    public static class DailyTimeBlockByUserPerDay
    {
        public static IQueryBuilder FindDailyTimeBlockByUserPerDay(this IQueryBuilder builder, int companyUserId, double timeZone, DateTime from, DateTime to)
        {
            builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
                .Select(" TO_JSON_STRING(STRUCT(cu.id AS userID, DATE(dtba.DateDay) AS Date, COUNT(DISTINCT g.id) AS GoalsWorked," +
                " FORMAT_TIME('%H:%M:%S',TIME(TIMESTAMP_SECONDS(SUM(TIMESTAMP_DIFF(dtba.time_end, dtba.time_start, SECOND))))) AS TotalHoursWorked,\n " +
                " FORMAT_TIME('%H:%M:%S',TIME(min(dtba.time_start))) as StartTime,\n " +
                " FORMAT_TIME('%H:%M:%S', TIME(Max(dtba.time_end))) as EndTime, \n" +
                " ARRAY_AGG(STRUCT(dtba.time_start AS StartTime,dtba.time_end AS EndTime , p.name As Project , t.Description AS Task)) AS TimeBlocks)) AS JsonData ")
                .From(" daily_time_block_adjusted dtba ")
                .Join(" public.task t ", " dtba.task_id = t.id ")
                .Join(" public.project p", " p.id = t.project_id")
                .Join(" public.goal g ", " t.goal_id = g.id ")
                .Join(" public.project_company_user pcu ", " pcu.id = dtba.owner_id ")
                .Join(" public.company_user cu ", " cu.id = pcu.company_user_id ")
                .Where($"cu.id = {companyUserId}")
                .Where($"extract(date from dtba.time_start) between '{from.ToString("yyy-MM-dd")}' and '{to.ToString("yyyy-MM-dd")}'")
                .GroupBy(" cu.id , dtba.DateDay ")
                .Having(" COUNT(DISTINCT g.id) > 0 ")
                .OrderBy(" dtba.DateDay ASC ");
            ;

            return builder;
        }
    }
}
