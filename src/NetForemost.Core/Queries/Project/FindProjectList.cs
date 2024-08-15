using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.Project
{
    public static class FindProjectList
    {
        public static IQueryBuilder GetProjects(this IQueryBuilder builder, int companyId)
        {
            // Build query to get Goals Name
            builder.Select("\np.id as ProjectId, p.name as Name  ")
                    .From("\npublic.project p")
                    .Join("\npublic.company c", "c.id = p.company_id")
                    .Where($"\np.company_id= {companyId}")
                    .OrderBy("\np.name asc");

            return builder;
        }

        public static IQueryBuilder GetProjectsPaginated(this IQueryBuilder builder, int perPage, int pageNumber)
        {
            // Add pagination
            builder.Limit(perPage).Offset(pageNumber, perPage);

            return builder;
        }
    }
}
