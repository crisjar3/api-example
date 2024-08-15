using CsvHelper.Configuration;
using NetForemost.Core.Entities.JobRoles;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class JobRoleCategoryMap : ClassMap<JobRoleCategory>
{
    public JobRoleCategoryMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}