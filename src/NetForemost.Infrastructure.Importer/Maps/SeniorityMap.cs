using CsvHelper.Configuration;
using NetForemost.Core.Entities.Seniorities;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class SeniorityMap : ClassMap<Seniority>
{
    public SeniorityMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}