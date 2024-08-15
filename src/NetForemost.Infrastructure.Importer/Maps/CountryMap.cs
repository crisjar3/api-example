using CsvHelper.Configuration;
using NetForemost.Core.Entities.Countries;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class CountryMap : ClassMap<Country>
{
    public CountryMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}