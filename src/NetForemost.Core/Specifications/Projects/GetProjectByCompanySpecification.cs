using Ardalis.Specification;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects;
public class GetProjectByCompanySpecification : Specification<Project>
{
    public GetProjectByCompanySpecification(int projectId, int companyId)
    {
        Query.Where(project => project.Id == projectId && project.CompanyId == companyId);
    }
}

