using Microsoft.AspNetCore.Http;

namespace NetForemost.API.Middlewares
{
    public static class MiddelwareExtension
    {
        public static void Set(this IHeaderDictionary headers, string name, string value)
        {
            if (headers.ContainsKey(name))
            {
                headers.Remove(name);
            }
            headers.Add(name, value);
        }
    }
}
