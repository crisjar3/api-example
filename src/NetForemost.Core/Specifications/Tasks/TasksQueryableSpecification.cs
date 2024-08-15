using Ardalis.Specification;
using NetForemost.Core.Dtos.Timer;

namespace NetForemost.Core.Specifications.Tasks
{
    public class TasksQueryableSpecification : Specification<Entities.Tasks.Task, GetTasksQueryableDto>
    {
        public TasksQueryableSpecification(
            string userId,
            string search,
            int typeId,
            int goalId,
            int projectId,
            int companyId,
            DateTime? targetEndDateFrom,
            DateTime? targetEndDateTo,
            int pageNumber, int perPage
        )
        {
            Query.Include(task => task.Project);
            Query.Include(task => task.Goal);
            Query.Include(task => task.Type);
            Query.Include(task => task.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);

            Query.Where(goal => goal.Owner.CompanyUser.UserId == userId);


            if (!string.IsNullOrEmpty(search))
            {
                Query.Where(task => task.Description.ToLower().Contains(search.ToLower()) || task.Type.Name.ToLower().Contains(search.ToLower()));
            }

            if (targetEndDateFrom is not null && targetEndDateTo is not null)
            {
                Query.Where(task => targetEndDateFrom >= task.TargetEndDate && targetEndDateTo <= task.TargetEndDate);
            }

            if (typeId > 0)
            {
                Query.Where(task => task.TypeId == typeId);
            }

            if (projectId > 0)
            {
                Query.Where(goal => goal.ProjectId == projectId);
            }

            if (goalId > 0)
            {
                Query.Where(task => task.GoalId == goalId);
            }

            if (companyId > 0)
            {
                Query.Where(task => task.CompanyId == companyId);
            }


            perPage = pageNumber == 0 ? 0 : perPage;
            Query.Skip((pageNumber - 1) * perPage).Take(perPage);

            Query.OrderBy(task => task.CreatedAt);

            Query.Select(task => new GetTasksQueryableDto
            {
                Id = task.Id,
                Description = task.Description,
                ProjectId = task.ProjectId,
                GoalId = task.GoalId,
                Type = new GetTypeTaskDto
                {
                    Id = task.Type.Id,
                    Description = task.Type.Description,
                },
                TimeSpentInSecond = Convert.ToInt32(task.TimeSpentInSecond)
            });
        }
    }
}