using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Queries.DailyTimeBlock;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.Goal;

public static class GetGoalsByProjectAndUserCompanyId
{
    public static IQueryBuilder GetGoal(this IQueryBuilder builder, IEnumerable<int> companiesUsersIds, int projectId, DateTime from, DateTime to, double timeZone)
    {
        builder.WithOpen(" daily_time_block_adjusted ")
           .Procedure(builder.FindDailyTimeBlockForTimeZone(timeZone).Build())
           .WithClose()
           .Select("g.id as GoalId, g.target_end_date as DueDate, g.has_extra_mile_goal as IsExtraMile, g.description as Description, \n IFNULL(cast(time(timestamp_seconds(sum(timestamp_diff(dtb.time_end, dtb.time_start, second)))) as string), '00:00:00')   as TimeWorked, \n " +
                       "IFNULL(cast(time(timestamp_seconds(cast((g.estimated_hours * 3600) as int64))) as string), '00:00:00')  as EstimatedTime, sp.points as Points, pl.level as Priority, gs.name as Status \n")
                   .From("public.goal g \n ")
                   .Join("public.goal_extra_mile ge", "ge.goal_id = g.id \n ", " LEFT ")
                   .Join("public.story_point sp", "g.story_point_id = sp.id \n ")
                   .Join("public.goal_status gs", "g.goal_status_id = gs.id or ge.goal_status_id = gs.id \n")
                   .Join("public.priority_level_translation pl", "g.priority_level_id = pl.id \n");

        if (!companiesUsersIds.IsNullOrEmpty())
            builder.Join("public.project_company_user pcu", "g.owner_id = pcu.id \n").Join("public.company_user cu", "cu.id = pcu.company_user_id \n");

        builder.Join("public.task t", "t.goal_id = g.id \n", "LEFT")
                .Join("daily_time_block_adjusted dtb", "dtb.task_id = t.id \n", "LEFT")
                .Where($"g.project_id = {projectId} \n")
                .Where($"(extract(Date FROM dtb.time_start) between '{from.ToString("yyy-MM-dd")}' and '{to.ToString("yyyy-MM-dd")}') \n");

        // Verify if companies is null or empty
        if (!companiesUsersIds.IsNullOrEmpty()) builder.Where($"cu.id").In(companiesUsersIds);

        builder.GroupBy("\n g.id, g.description, g.target_end_date, g.estimated_hours, sp.points, pl.level, gs.name, g.has_extra_mile_goal")
                .OrderBy("\n g.target_end_date asc");

        return builder;
    }


    public static IQueryBuilder GetGoalPaginated(this IQueryBuilder builder, int perPage, int pageNumber)
    {
        // Add pagination
        builder.Limit(perPage).Offset(pageNumber, perPage);

        return builder;
    }
}