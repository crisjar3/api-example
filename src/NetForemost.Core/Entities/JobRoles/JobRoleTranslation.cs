using NetForemost.Core.Entities.Languages;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobRoles;

public class JobRoleTranslation : BaseEntity
{
    public int JobRoleId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual Language Language { get; set; }
    public virtual JobRole JobRole { get; set; }
}