using Ardalis.Result;
using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Interfaces.Cities;
using NetForemost.Core.Specifications.Cities;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Cities
{
    public class CityService : ICityService
    {
        private readonly IAsyncRepository<City> _cityRepository;
        private readonly IAsyncRepository<Country> _countryRepository;

        public CityService(IAsyncRepository<City> cityRepository, IAsyncRepository<Country> countryRepository)
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
        }

        public async Task<Result<List<City>>> FindCitiesAsync(int countryId)
        {
            try
            {
                var country = await _countryRepository.GetByIdAsync(countryId);

                //verify if the country exist
                if (country is null)
                {
                    return Result<List<City>>.Invalid(
                        new()
                        {
                            new()
                            {
                                ErrorMessage = ErrorStrings.CountryNotFound
                            }
                        });
                }

                //Get cities by the country
                var cities = await _cityRepository.ListAsync(new GetCityByCountrySpecification(countryId));

                //Verify if exist any cities
                if (cities.IsNullOrEmpty())
                    return Result<List<City>>.Invalid(
                        new()
                        {
                            new()
                            {
                                ErrorMessage = ErrorStrings.CityNotFound
                            }
                        });

                return Result<List<City>>.Success(cities);
            }
            catch (Exception ex)
            {
                return Result<List<City>>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }
    }
}
