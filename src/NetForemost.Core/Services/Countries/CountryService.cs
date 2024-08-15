using Ardalis.Result;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Interfaces.Countries;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;

namespace NetForemost.Core.Services.Countries
{
    public class CountryService : ICountryService
    {
        private readonly IAsyncRepository<Country> _countryRepository;

        public CountryService(IAsyncRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Result<List<Country>>> FindCountriesAsync()
        {
            try
            {
                var countries = await _countryRepository.ListAsync();
                return Result<List<Country>>.Success(countries);
            }
            catch (Exception ex)
            {
                return Result<List<Country>>.Error(ErrorHelper.GetExceptionError(ex));
            }
        }
    }
}
