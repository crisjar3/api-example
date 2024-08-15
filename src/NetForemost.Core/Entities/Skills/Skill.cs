using NetForemost.Core.Entities.JobOffers;
using NetForemost.Core.Entities.JobRoles;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Skills
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<JobOfferTalentSkill> JobOfferJobRoleSkills { get; set; }
        public virtual ICollection<JobRoleCategorySkill> JobRoleCategorySkills { get; set; }
    }
}
