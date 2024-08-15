using Ardalis.Specification;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects;

public class GetCompanyProjectsSpecification : Specification<Project, ProjectDtoSimple>
{
    public GetCompanyProjectsSpecification(int[] projectIds, int companyId)
    {
        Query.Where(project =>
                                projectIds.Contains(project.Id) &&
                                project.CompanyId == companyId
                   );

        Query.Select(project => new ProjectDtoSimple
        {
            Id = project.Id,
            Name = project.Name,
        });
    }
}
