using CsvHelper.Configuration;
using NetForemost.Core.Entities.Industries;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class IndustriesMap : ClassMap<Industry>
{
    public IndustriesMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}