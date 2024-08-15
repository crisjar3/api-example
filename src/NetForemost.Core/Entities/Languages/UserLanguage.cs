using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Languages;
public class UserLanguage : BaseEntity
{
    public string UserId { get; set; }
    public int LanguageId { get; set; }
    public int LanguageLevelId { get; set; }

    public virtual User User { get; set; }
    public virtual Language Language { get; set; }
    public virtual LanguageLevel LanguageLevel { get; set; }
}
