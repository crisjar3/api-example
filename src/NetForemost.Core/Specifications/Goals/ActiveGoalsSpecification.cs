using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals
{
    public class ActiveGoalsSpecification : Specification<Goal, Goal>
    {
        public ActiveGoalsSpecification(string userId, double timeZone, bool includeRelations = false)
        {
            if (includeRelations)
            {
                Query.Include(goal => goal.Project);
                Query.Include(goal => goal.ScrumMaster);
                Query.Include(goal => goal.StoryPoint);
                Query.Include(goal => goal.PriorityLevel);
                Query.Include(goal => goal.GoalStatus).ThenInclude(goalStatus => goalStatus.StatusCategory);
                Query.Include(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);
            }


            Query.Where(
                goal => goal.Owner.CompanyUser.UserId == userId &&
                        goal.ActualEndDate == null &&
                        goal.HasExtraMileGoal == false
            );

            Query.Select(goal => new Goal()
            {
                Id = goal.Id,
                Name = goal.Name,
                StartDate = goal.StartDate.AddHours(timeZone),
                TargetEndDate = goal.TargetEndDate.AddHours(timeZone),
                ActualEndDate = goal.ActualEndDate.Value.AddHours(timeZone),
                HasExtraMileGoal = goal.HasExtraMileGoal,
                Approved = goal.Approved,
                Description = goal.Description,
                EstimatedHours = goal.EstimatedHours,
                StoryPointId = goal.StoryPointId,
                StoryPoint = goal.StoryPoint,
                JiraTicketId = goal.JiraTicketId,
                PriorityLevelId = goal.PriorityLevelId,
                PriorityLevel = goal.PriorityLevel,
                ProjectId = goal.ProjectId,
                Project = goal.Project,
                OwnerId = goal.OwnerId,
                Owner = goal.Owner,
                ScrumMasterId = goal.ScrumMasterId,
                ScrumMaster = goal.ScrumMaster,
                GoalStatusId = goal.GoalStatusId
            });

            Query.OrderBy(goal => goal.TargetEndDate);
        }

    }
}
