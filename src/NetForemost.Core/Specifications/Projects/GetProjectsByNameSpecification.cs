using Ardalis.Specification;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects;
public class GetProjectsByNameSpecification : Specification<Project>
{
    public GetProjectsByNameSpecification(string name, int companyId)
    {
        Query.Where(project => project.Name == name && project.CompanyId == companyId);
    }
}
