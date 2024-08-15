using CsvHelper.Configuration;
using NetForemost.Core.Entities.Skills;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class SkillMap : ClassMap<Skill>
{
    public SkillMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}