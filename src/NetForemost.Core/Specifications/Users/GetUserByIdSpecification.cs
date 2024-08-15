using Ardalis.Specification;

namespace NetForemost.Core.Specifications.Users;

public class GetUserByIdSpecification : IncludeUserEntitiesSpecification
{
    /// <summary>
    /// Obtain the user by Id.
    /// </summary>
    /// <param name="userId"></param>
    public GetUserByIdSpecification(string userId)
    {
        Query.Where(user => user.Id == userId);
    }
}
