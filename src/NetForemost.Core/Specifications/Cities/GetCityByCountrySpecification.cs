using Ardalis.Specification;
using NetForemost.Core.Entities.Countries;

namespace NetForemost.Core.Specifications.Cities;
public class GetCityByCountrySpecification : Specification<City>
{
    /// <summary>
    /// Obtains the city by the country id.
    /// </summary>
    /// <param name="countryId">The userId.</param>
    public GetCityByCountrySpecification(int countryId)
    {
        Query.Where(city => city.CountryId == countryId)
            .OrderBy(city => city.Name);
    }
}

