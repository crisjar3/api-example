using CsvHelper.Configuration;
using NetForemost.Core.Entities.Roles;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class RoleTranslationMap : ClassMap<RoleTranslation>
{
    public RoleTranslationMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();

        Map(m => m.Role.Id).Ignore();
        Map(m => m.Role.Name).Ignore();
        Map(m => m.Role.NormalizedName).Ignore();
        Map(m => m.Role.ConcurrencyStamp).Ignore();
        Map(m => m.Role.CreatedAt).Ignore();
        Map(m => m.Role.CreatedBy).Ignore();
        Map(m => m.Role.UpdatedAt).Ignore();
        Map(m => m.Role.UpdatedBy).Ignore();

        Map(m => m.Language).Ignore();
        Map(m => m.Language.IsActive).Ignore();
        Map(m => m.Language.Code).Ignore();
        Map(m => m.Language.Name).Ignore();
        Map(m => m.Language.Description).Ignore();
        Map(m => m.Language.CreatedAt).Ignore();
        Map(m => m.Language.CreatedBy).Ignore();
        Map(m => m.Language.UpdatedAt).Ignore();
        Map(m => m.Language.UpdatedBy).Ignore();
    }
}