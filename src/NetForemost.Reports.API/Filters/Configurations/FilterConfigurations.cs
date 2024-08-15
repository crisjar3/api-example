namespace NetForemost.Report.API.Filters.Configurations
{
    public static class FilterConfigurations
    {
        public static bool ApplyFilter(List<string> endpoints, string currentEndpoint)
        {
            return endpoints.Any(endpoint => endpoint.Equals("/" + currentEndpoint));
        }
    }
}