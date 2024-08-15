using NetForemost.Core.Entities.Languages;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobRoles;

public class JobRoleCategoryTranslation : BaseEntity
{
    public int JobRoleCategoryId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual Language Language { get; set; }
    public virtual JobRoleCategory JobRoleCategory { get; set; }
}