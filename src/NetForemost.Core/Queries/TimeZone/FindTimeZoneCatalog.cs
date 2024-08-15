using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.TimeZone
{
    public static class FindTimeZoneCatalog
    {
        public static IQueryBuilder TimeZoneCatalog(this IQueryBuilder builder)
        {
            builder.Select("id as Id ,offset as Offset , text as Text")
                .From("public.time_zone")
                .OrderBy("id ASC");

            return builder;
        }
    }
}
