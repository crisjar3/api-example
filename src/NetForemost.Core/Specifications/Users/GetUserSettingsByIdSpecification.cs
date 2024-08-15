using Ardalis.Specification;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Specifications.Users;

public class GetUserSettingsByIdSpecification : Specification<UserSettings>, ISingleResultSpecification
{
    public GetUserSettingsByIdSpecification(string userId)
    {
        Query.Where(x => x.UserId == userId);
    }
}