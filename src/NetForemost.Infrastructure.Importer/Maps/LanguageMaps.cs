﻿using CsvHelper.Configuration;
using NetForemost.Core.Entities.Languages;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class LanguageMaps : ClassMap<Language>
{
    public LanguageMaps()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}