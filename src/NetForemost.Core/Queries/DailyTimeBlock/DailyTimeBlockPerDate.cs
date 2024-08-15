using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.DailyTimeBlock
{
    public static class DailyTimeBlockPerDate
    {
        public static IQueryBuilder GetDailyTimeBBlockPerDate(this IQueryBuilder builder, DateTime date, int companyUser, double timeZone)
        {
            builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
                .Select("dtb.id as Id, pr.name as Project, dtb.time_start as TimeStart,  dtb.time_end as TimeEnd, t.description as TaskDescription, g.name as Goal, tt.name as TaskType," +
               " CONCAT( CAST(DATE_DIFF(dtb.time_end, dtb.time_start, HOUR) AS STRING),'h', ' ',  CAST(MOD(DATE_DIFF(dtb.time_end, dtb.time_start, MINUTE), 60) AS STRING),'m') as TotalWorkedInHours")
               .From(" daily_time_block_adjusted dtb")
               .Join(" public.task t", "t.id = dtb.task_id")
               .Join(" public.project_company_user pcu ", " pcu.id = t.owner_id ")
               .Join(" public.project pr ", "pr.id = pcu.project_id")
               .Join(" public.goal g", "g.id = t.goal_id ")
               .Join("public.task_type tt", "tt.id = t.type_id ")
               .Where($" pcu.company_user_id = {companyUser} and dtb.DateDay = '{date.ToString("yyyy-MM-dd")}' ");

            return builder;

        }
    }
}
