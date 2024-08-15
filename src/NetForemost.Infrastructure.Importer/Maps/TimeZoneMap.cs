using CsvHelper.Configuration;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class TimeZoneMap : ClassMap<Core.Entities.TimeZones.TimeZone>
{
    public TimeZoneMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}