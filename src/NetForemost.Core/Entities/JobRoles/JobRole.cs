using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.JobOffers;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.JobRoles;

public class JobRole : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int JobRoleCategoryId { get; set; }
    public bool IsDefault { get; set; }
    public int? CompanyId { get; set; }

    public JobRoleCategory JobRoleCategory { get; set; }
    public virtual ICollection<JobRoleTranslation> JobRoleTranslations { get; set; }
    public virtual ICollection<JobOfferTalent> JobOfferJobRoles { get; set; }
    public virtual Company? Company { get; set; }
}