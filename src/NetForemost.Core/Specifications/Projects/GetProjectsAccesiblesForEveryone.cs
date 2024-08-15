using Ardalis.Specification;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects;

public class GetProjectsAccesiblesForEveryone : Specification<Project>
{
    public GetProjectsAccesiblesForEveryone()
    {
        Query.Where(project => project.IsAccessibleForEveryone);
    }
}