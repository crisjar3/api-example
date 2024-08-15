using Ardalis.Specification;
using NetForemost.Core.Dtos.Tasks;
using NetForemost.Core.Entities.Tasks;

namespace NetForemost.Core.Specifications.Tasks
{
    public class TaskTypesQueryableSpecification : Specification<TaskType, GetTaskTypesDto>
    {
        public TaskTypesQueryableSpecification(
            string name,
            string description,
            int projectId,
            int companyId,
            int pageNumber, int perPage
        )
        {

            if (!string.IsNullOrEmpty(name))
            {
                Query.Search(task => task.Name.ToLower(), "%" + name.ToLower() + "%");
            }

            if (!string.IsNullOrEmpty(description))
            {
                Query.Search(task => task.Description.ToLower(), "%" + description.ToLower() + "%");
            }

            if (projectId > 0)
            {
                Query.Where(task => task.ProjectId == projectId);
            }

            if (companyId > 0)
            {
                Query.Where(task => task.CompanyId == companyId);
            }


            perPage = pageNumber == 0 ? 0 : perPage;
            Query.Skip((pageNumber - 1) * perPage).Take(perPage);

            Query.Select(task => new GetTaskTypesDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
            });
        }
    }
}
