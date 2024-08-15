using Ardalis.Result;
using NetForemost.Core.Entities.Countries;

namespace NetForemost.Core.Interfaces.Cities
{
    public interface ICityService
    {
        /// <summary>
        /// Obtain the cities by country.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns>The list of cities.</returns>
        Task<Result<List<City>>> FindCitiesAsync(int countryId);
    }
}
