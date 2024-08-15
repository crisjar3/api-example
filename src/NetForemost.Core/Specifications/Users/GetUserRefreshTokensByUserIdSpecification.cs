using Ardalis.Specification;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Specifications.Users;

public class GetUserRefreshTokensByUserIdSpecification : Specification<UserRefreshToken>, ISingleResultSpecification
{
    public GetUserRefreshTokensByUserIdSpecification(string userId)
    {
        Query.Where(userRefreshToken => userRefreshToken.UserId == userId && userRefreshToken.Active);
    }
}