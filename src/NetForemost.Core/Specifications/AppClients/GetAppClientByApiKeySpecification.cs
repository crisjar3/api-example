using Ardalis.Specification;
using NetForemost.Core.Entities.AppClients;

namespace NetForemost.Core.Specifications.AppClients;

public class GetAppClientByApiKeySpecification : Specification<AppClient>
{
    /// <summary>
    /// Obtains the AppClient by the api key.
    /// </summary>
    /// <param name="apiKey">The api key.</param>
    public GetAppClientByApiKeySpecification(Guid apiKey)
    {
        Query.Where(appClient => appClient.ApiKey == apiKey);
    }
}
