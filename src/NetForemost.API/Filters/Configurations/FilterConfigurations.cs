using System.Collections.Generic;
using System.Linq;
using static NetForemost.API.Endpoints.Routes;

namespace NetForemost.API.Filters.Configurations
{
    public static class FilterConfigurations
    {
        public readonly static List<string> LanguageEndpoints = new()
        {
            PriorityLevels.GetPriorityLevels,
            Authentication.PostLogin
        };

        public static bool ApplyFilter(List<string> endpoints, string currentEndpoint)
        {
            return endpoints.Any(endpoint => endpoint.Equals("/" + currentEndpoint));
        }
    }
}