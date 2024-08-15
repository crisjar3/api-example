using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetForemost.Core.Dtos.Projects
{
    public class ProjectCompanyUserSettingsDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }
        public string ProjectName { get; set; }
    }
}
