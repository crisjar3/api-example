﻿using CsvHelper.Configuration;
using NetForemost.Core.Entities.Goals;
using System.Globalization;

namespace NetForemost.Infrastructure.Importer.Maps;

public class GoalStatusCategoryMap : ClassMap<GoalStatusCategory>
{
    public GoalStatusCategoryMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.UpdatedAt).Ignore();
        Map(m => m.UpdatedBy).Ignore();
        Map(m => m.CreatedAt).Ignore();
        Map(m => m.CreatedBy).Ignore();
    }
}