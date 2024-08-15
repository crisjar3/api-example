using CsvHelper.Configuration;
using NetForemost.Core.Entities.AppClients;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;
public class AppClientMap : ClassMap<AppClient>
{
    public AppClientMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}

