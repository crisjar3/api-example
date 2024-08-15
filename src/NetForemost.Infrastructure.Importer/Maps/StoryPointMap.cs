using CsvHelper.Configuration;
using NetForemost.Core.Entities.StoryPoints;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class StoryPointMap : ClassMap<StoryPoint>
{
    public StoryPointMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}