using Ardalis.Specification;
using NetForemost.Core.Entities.Tasks;


namespace NetForemost.Core.Specifications.Tasks
{
    public class TaskTypesByNameSpecification : Specification<TaskType>
    {
        public TaskTypesByNameSpecification(string name, int companyId, int projectId)
        {
            Query.Where(taskType => taskType.Name == name &&
                                    taskType.CompanyId == companyId &&
                                    taskType.ProjectId == projectId
                       );
        }
    }
}
