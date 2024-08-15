using CsvHelper.Configuration;
using NetForemost.Core.Entities.PriorityLevels;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class PriorityLevelMap : ClassMap<PriorityLevel>
{
    public PriorityLevelMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}