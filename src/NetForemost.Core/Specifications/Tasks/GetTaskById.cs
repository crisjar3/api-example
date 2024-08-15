using Ardalis.Specification;
using NetForemost.Core.Dtos.Timer;

namespace NetForemost.Core.Specifications.Tasks;

public class GetTaskById : Specification<NetForemost.Core.Entities.Tasks.Task, GetTasksQueryableDto>
{
    public GetTaskById(int taskId, string userId)
    {
        Query.Include(task => task.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);
        Query.Where(task => task.Id == taskId && task.Owner.CompanyUser.UserId == userId)
            .Include(task => task.Type);

        Query.Select(task => new GetTasksQueryableDto
        {
            Id = task.Id,
            Description = task.Description,
            ProjectId = task.ProjectId,
            GoalId = task.GoalId,
            Type = new GetTypeTaskDto
            {
                Id = task.Type.Id,
                Description = task.Description,
            }
        });
    }
}
