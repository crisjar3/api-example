using CsvHelper.Configuration;
using NetForemost.Core.Entities.Countries;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class CityMap : ClassMap<City>
{
    public CityMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();

        Map(m => m.Country.OfficialName).Ignore();
        Map(m => m.Country.CountryCode).Ignore();
        Map(m => m.Country.UpdatedAt).Ignore();
        Map(m => m.Country.UpdatedBy).Ignore();
        Map(m => m.Country.CreatedAt).Ignore();
        Map(m => m.Country.CreatedBy).Ignore();
    }
}