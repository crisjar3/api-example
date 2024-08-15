using Ardalis.Specification;
using TaskEntity = NetForemost.Core.Entities.Tasks.Task;

namespace NetForemost.Core.Specifications.Tasks;

public class GetTaskByUserId : Specification<TaskEntity>
{
    public GetTaskByUserId(string userId, int taskId)
    {
        Query.Include(task => task.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);
        Query.Where(task => task.Id == taskId && task.Owner.CompanyUser.UserId == userId)
            .Include(task => task.Goal);
    }
}