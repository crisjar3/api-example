using NetForemost.Core.Dtos.Skills;

namespace NetForemost.Core.Dtos.JobRoles;

public class JobRoleCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<JobRoleDto> JobRoles { get; set; }
    public List<SkillDto> Skills { get; set; }
}
