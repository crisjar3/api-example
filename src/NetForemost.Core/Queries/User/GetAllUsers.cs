using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Queries.User;

public static class GetAllUsers
{
    public static IQueryBuilder FindAllUsers(this IQueryBuilder builder, int companyId)
    {
        builder.Select(" cu.id as Id, u.user_name as UserName ")
            .From(" `public.AspNetUsers` AS u ")
            .Join(" public.company_user as cu ", " u.id = cu.user_id ")
            .Where($" cu.company_id = {companyId} ");
        return builder;
    }

    public static IQueryBuilder FindAllUsersPaginated(this IQueryBuilder builder, int perPage, int pageNumber)
    {
        // Add pagination
        builder.Limit(perPage).Offset(pageNumber, perPage);

        return builder;
    }
}
