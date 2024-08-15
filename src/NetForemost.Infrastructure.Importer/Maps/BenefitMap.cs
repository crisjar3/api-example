using CsvHelper.Configuration;
using NetForemost.Core.Entities.Benefits;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class BenefitMap : ClassMap<Benefit>
{
    public BenefitMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
        Map(m => m.CompanyId).Ignore();
    }
}