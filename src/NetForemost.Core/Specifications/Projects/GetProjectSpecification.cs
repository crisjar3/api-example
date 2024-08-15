using Ardalis.Specification;
using NetForemost.Core.Dtos.Projects;
using NetForemost.Core.Entities.Projects;

namespace NetForemost.Core.Specifications.Projects
{
    /// <summary>
    /// Obtains the projects by several fields.
    /// </summary>
    /// <param name="name">The name of projects.</param>
    /// <param name="description">The name of projects.</param>
    /// <param name="companyId">The id of the company owning the project.</param>
    /// <param name="techStack">The list of technologies used in the project.</param>
    /// <param name="budgetRangeFrom">The range budget of the project.</param>
    /// <param name="budgetRangeTo">The range budget of the project.</param>
    /// <param name="dateEndFrom">The estimated completion date range.</param>
    /// <param name="dateEndTo">The estimated completion date range.</param>
    /// <param name="dateStartTo">The project start date range.</param>
    /// <param name="dateStartFrom">The project start date range.</param>
    /// <param name="pageNumber">Page number to display</param>
    /// <param name="perPage">Number of records per page</param>
    /// <param name="paginate">to decide where paginate the results</param>
    public class GetProjectSpecification : Specification<Project, ProjectStatusDto>
    {
        public GetProjectSpecification(
            string userId,
            int projectId,
            string name, string description,
            int companyId, string[] techStack,
            float budgetRangeFrom, float budgetRangeTo,
            DateTime? dateStartTo, DateTime? dateStartFrom,
            DateTime? dateEndTo, DateTime? dateEndFrom,
            int pageNumber, int perPage, bool paginate
            )
        {
            //Query.Include(project => project.ProjectAvatar);
            Query.Where(project => project.CompanyId == companyId && (string.IsNullOrEmpty(userId) || project.ProjectCompanyUsers.Any(pcu => pcu.CompanyUser.UserId == userId)) && !project.isDeleted);

            if (projectId > 0)
            {
                Query.Where(pr => pr.Id == projectId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                Query.Search(project => project.Name.ToLower(), "%" + name.ToLower() + "%");
            }

            if (!string.IsNullOrEmpty(description))
            {
                Query.Search(project => project.Description.ToLower(), "%" + description.ToLower() + "%");
            }

            if (techStack is not null)
            {
                Query.Where(project => project.TechStack.Any(technology => techStack.Any(tech => tech.Equals(technology))));
            }

            if (dateStartFrom is not null && dateStartTo is not null)
            {
                Query.Where(project => dateStartFrom >= project.StartedDate && dateStartTo <= project.StartedDate);
            }

            if (dateEndFrom is not null && dateEndTo is not null)
            {
                Query.Where(project => dateEndFrom >= project.EndEstimatedDate && dateEndTo <= project.EndEstimatedDate);
            }

            if (budgetRangeFrom > 0)
            {
                Query.Where(project => project.Budget >= budgetRangeFrom && project.Budget <= budgetRangeTo);
            }

            if (paginate)
            {
                Query.Skip((pageNumber - 1) * perPage).Take(perPage);
            }

            Query.Select(proj => new ProjectStatusDto()
            {
                Id = proj.Id,
                Name = proj.Name,
                Description = proj.Description,
                ProjectImageUrl = proj.ProjectImageUrl,
                IsActive = (!proj.IsArchived && !proj.isDeleted),
                IsArchived = proj.IsArchived,
                IsAccessibleForEveryone = proj.IsAccessibleForEveryone
            });

            Query.OrderBy(project => project.Name);
        }
    }
}
