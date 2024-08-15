using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetForemost.Core.Dtos.Projects
{
    public class ProjectAvatarDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AvatarImageUrl { get; set; }
    }
}
