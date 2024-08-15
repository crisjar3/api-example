using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Languages;

public class LanguageLevel : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}