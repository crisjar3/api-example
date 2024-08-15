using Ardalis.Specification;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Specifications.Projects
{
    public class GetProjectAvatarByProjectIdSpecification : Specification<NetForemost.Core.Entities.Projects.ProjectAvatar>
    {
        public GetProjectAvatarByProjectIdSpecification(int projectId)
        {
            Query.Where(projectAvatar => projectAvatar.ProjectId== projectId);
        }        
    }
}
