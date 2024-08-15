using NetForemost.Core.Entities.Languages;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Roles;

public class RoleTranslation : BaseEntity
{
    public string RoleId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; }

    public virtual Language Language { get; set; }
    public virtual Role Role { get; set; }
}
