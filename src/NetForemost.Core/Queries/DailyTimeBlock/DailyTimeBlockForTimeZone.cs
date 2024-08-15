using NetForemost.SharedKernel.Interfaces;
namespace NetForemost.Core.Queries.DailyTimeBlock;

public static class DailyTimeBlockForTimeZone
{
    public static IQueryBuilder FindDailyTimeBlockForTimeZone(this IQueryBuilder builder, double timezone)
    {
        builder.Select("id," +
            $" \nTIMESTAMP_ADD(time_start, INTERVAL {timezone} HOUR) AS time_start," +
            $" \nCASE" +
            $" \nWHEN DATE_TRUNC(TIMESTAMP_ADD(time_start, INTERVAL {timezone} HOUR), DAY) = DATE_TRUNC(TIMESTAMP_ADD(time_end, INTERVAL {timezone} HOUR), DAY)" +
            $" \nTHEN TIMESTAMP_ADD(time_end , INTERVAL {timezone} HOUR) " +
            $" \nELSE TIMESTAMP(FORMAT_TIMESTAMP( '%F 23:59:59' ,TIMESTAMP_ADD(time_start, INTERVAL {timezone} HOUR)))" +
            $" \nEND AS time_end," +
            $" \nCASE" +
            $" \nWHEN DATE_TRUNC(TIMESTAMP_ADD(time_start, INTERVAL {timezone} HOUR), DAY) != DATE_TRUNC(TIMESTAMP_ADD(time_end, INTERVAL {timezone} HOUR), DAY) " +
            $" \nTHEN Date(TIMESTAMP_add(time_start, INTERVAL {timezone} HOUR)) " +
            $" \nelse Date(TIMESTAMP_add(time_start, INTERVAL {timezone} HOUR))" +
            $" \nEND AS DateDay," +
            $" \ntask_id," +
            $" \ndevice_id, " +
            $" \nowner_id")
            .From("\npublic.daily_time_block")
            .Union(" \nDISTINCT ")
            .Select($" \nid, " +
            $" \nCASE " +
            $" \nWHEN DATE_TRUNC(TIMESTAMP_ADD(time_start, INTERVAL {timezone} HOUR), DAY) = DATE_TRUNC(TIMESTAMP_ADD(time_end, INTERVAL {timezone} HOUR), DAY) " +
            $" \nTHEN TIMESTAMP_ADD(time_start , INTERVAL {timezone} HOUR)" +
            $" \nELSE TIMESTAMP(FORMAT_TIMESTAMP( '%F 0:0:0' ,TIMESTAMP_ADD(time_end, INTERVAL {timezone} HOUR)))" +
            $" \nEND AS time_start," +
            $" \nTIMESTAMP_add(time_end, INTERVAL {timezone} HOUR) AS time_end," +
            $" \nCASE" +
            $" \nWHEN DATE_TRUNC(TIMESTAMP_ADD(time_start, INTERVAL {timezone} HOUR), DAY) != DATE_TRUNC(TIMESTAMP_ADD(time_end, INTERVAL {timezone} HOUR), DAY)" +
            $" \nTHEN Date(TIMESTAMP_add(time_end, INTERVAL {timezone} HOUR))" +
            $" \nelse  Date(TIMESTAMP_add(time_end, INTERVAL {timezone} HOUR))" +
            $" \nEND AS DateDay," +
            $" \ntask_id," +
            $" \ndevice_id," +
            $" \nowner_id ")
            .From("\npublic.daily_time_block");

        return builder;
    }
}
