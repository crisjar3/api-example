using NetForemost.Core.Queries.DailyTimeBlock;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.Goal;

public static class GetGoalData
{
    public static IQueryBuilder GetGoal(this IQueryBuilder builder, int goalId, double timeZone)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
                .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
                .WithClose()
            .Select(" g.id AS GoalId, g.name AS GoalName, g.description AS Description, g.start_date AS StartDate, g.actual_end_date AS EndDate, pl.level AS PriorityLevel, " +
               " \nCONCAT(CAST(FLOOR(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND) / 3600)) AS STRING),'h ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)),3600) / 60) AS STRING),'m ',CAST(FLOOR(MOD(SUM(TIMESTAMP_DIFF(dtb.time_end, dtb.time_start, SECOND)), 60))  AS STRING) ,'s') AS TimeWorked, " +
               " \ncu.user_name AS UserName ")
               .From(" \npublic.goal as g")
               .Join(" \npublic.project_company_user pcu ", " pcu.id = g.owner_id ")
               .Join(" \npublic.company_user cu ", " cu.id = pcu.company_user_id ")
               .Join(" \npublic.priority_level AS pl ", " g.priority_level_id = pl.id ")
               .Join(" \npublic.task AS t ", " t.goal_id = g.id ")
               .Join(" \ndaily_time_block_adjusted AS dtb ", " dtb.task_id = t.id ")
               .Where($" g.id = {goalId} ")
               .GroupBy(" g.id , g.name,g.description, g.start_date, g.actual_end_date,pl.level, pl.id , cu.id , cu.user_name ");

        return builder;
    }
}
