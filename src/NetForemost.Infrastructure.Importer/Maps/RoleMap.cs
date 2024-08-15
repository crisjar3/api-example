using CsvHelper.Configuration;
using NetForemost.Core.Entities.Roles;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class RoleMap : ClassMap<Role>
{
    public RoleMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}