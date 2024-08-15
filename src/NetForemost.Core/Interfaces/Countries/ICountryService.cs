using Ardalis.Result;
using NetForemost.Core.Entities.Countries;

namespace NetForemost.Core.Interfaces.Countries
{
    public interface ICountryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>the list of countries</returns>
        Task<Result<List<Country>>> FindCountriesAsync();
    }
}
