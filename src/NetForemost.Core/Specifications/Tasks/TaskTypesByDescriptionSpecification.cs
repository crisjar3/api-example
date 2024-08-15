using Ardalis.Specification;
using NetForemost.Core.Entities.Tasks;


namespace NetForemost.Core.Specifications.Tasks
{
    public class TaskTypesByDescriptionSpecification : Specification<TaskType>
    {
        public TaskTypesByDescriptionSpecification(string description, int companyId, int projectId)
        {
            Query.Where(taskType => taskType.Description == description &&
                                    taskType.CompanyId == companyId &&
                                    taskType.ProjectId == projectId
                        );
        }
    }
}
