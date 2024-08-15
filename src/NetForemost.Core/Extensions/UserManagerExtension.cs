using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Extensions;
public static class UserManagerExtensions
{
    public static IQueryable<T> ImplementSpecification<T>(this UserManager<T> userManager, ISpecification<T> specification) where T : User
    {
        var query = SpecificationEvaluator.Default.GetQuery(userManager.Users.AsQueryable(), specification);

        return query;
    }

}