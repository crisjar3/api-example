using Ardalis.Specification;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Specifications.Users;

public class GetUserRefreshTokenByRefreshTokenSpecification : Specification<UserRefreshToken>,
    ISingleResultSpecification
{
    public GetUserRefreshTokenByRefreshTokenSpecification(string userId, string refreshToken)
    {
        Query.Where(x => x.UserId == userId)
            .Where(x => x.Value == refreshToken)
            .Where(x => x.Active);
    }
}