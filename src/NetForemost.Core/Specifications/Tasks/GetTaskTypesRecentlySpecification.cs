using Ardalis.Specification;
using NetForemost.Core.Dtos.Tasks;
using Task = NetForemost.Core.Entities.Tasks.Task;

namespace NetForemost.Core.Specifications.Tasks
{
    public class GetTaskTypesRecentlySpecification : Specification<Task, GetTaskTypesDto>
    {
        public GetTaskTypesRecentlySpecification(int companyUserId, int projectId, string search, int pageNumber, int perPage, bool paginate)
        {
            Query.Include(task => task.Owner);
            Query.Include(task => task.Type);

            Query.Where(task => task.Owner.CompanyUserId == companyUserId && task.Type.ProjectId == projectId);


            if (paginate)
            {
                Query.Skip((pageNumber - 1) * perPage).Take(perPage);
            }

            if (search is not null)
            {
                Query.Where(task => task.Type.Name.ToUpper().Contains(search.ToUpper()));
            }

            Query.Select(task => new GetTaskTypesDto()
            {
                Id = task.TypeId,
                Name = task.Type.Name,
                Description = task.Type.Description
            });

            Query.OrderByDescending(task => task.CreatedAt);
        }
    }
}


