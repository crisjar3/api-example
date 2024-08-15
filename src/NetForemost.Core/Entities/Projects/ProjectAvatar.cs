using NetForemost.SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetForemost.Core.Entities.Projects
{
    public class ProjectAvatar:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? AvatarImageUrl { get; set; }
        public int? ProjectId { get; set; }

        public virtual Project? Project { get; set; }
    }
}
