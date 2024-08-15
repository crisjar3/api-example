using CsvHelper.Configuration;
using NetForemost.Core.Entities.JobRoles;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class JobRoleMap : ClassMap<JobRole>
{
    public JobRoleMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();

        Map(m => m.JobRoleCategory).Ignore();
        Map(m => m.JobRoleCategory.Id).Ignore();
        Map(m => m.JobRoleCategory.Name).Ignore();
        Map(m => m.JobRoleCategory.Description).Ignore();

        Map(m => m.JobRoleCategory.UpdatedAt).Ignore();
        Map(m => m.JobRoleCategory.UpdatedBy).Ignore();
        Map(m => m.JobRoleCategory.CreatedAt).Ignore();
        Map(m => m.JobRoleCategory.CreatedBy).Ignore();
    }
}