using Ardalis.Specification;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects;
public class GetProjectByDateSpecification : Specification<Project>
{
    /// <summary>
    /// Obtain all projects in a date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public GetProjectByDateSpecification(DateTime startDate, DateTime endDate)
    {
        Query.Where(project => project.CreatedAt <= startDate && project.CreatedAt >= endDate);
    }
}
