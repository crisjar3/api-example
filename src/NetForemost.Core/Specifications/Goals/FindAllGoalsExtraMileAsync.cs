using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals
{
    public class FindAllGoalsExtraMileSpecification : Specification<GoalExtraMile, GoalExtraMile>
    {
        public FindAllGoalsExtraMileSpecification(
            string userId,
            int goalId,
            string description,
            int projectId,
            int storyPoints,
            DateTime? dateStartTo, DateTime? dateStartFrom,
            DateTime? actualendDateTo, DateTime? actualendDateFrom,
            string scrumMasterId,
            string jiraTicketId,
            string priorityLevel,
            int goalExtraMileStatusId,
            double timezone,
            int pageNumber, int perPage, bool paginate = false
        )
        {
            Query.Include(goalExtraMile => goalExtraMile.Goal);
            Query.Include(goalExtraMile => goalExtraMile.Goal.Project);
            Query.Include(goalExtraMile => goalExtraMile.Goal.ScrumMaster);
            Query.Include(goalExtraMile => goalExtraMile.Goal.StoryPoint);
            Query.Include(goalExtraMile => goalExtraMile.Goal.PriorityLevel);
            Query.Include(goalExtraMile => goalExtraMile.GoalStatus).ThenInclude(goalExtraMileStatus => goalExtraMileStatus.StatusCategory);
            Query.Include(goalExtraMile => goalExtraMile.Goal).ThenInclude(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);

            Query.Where(goalExtraMile => goalExtraMile.Goal.Owner.CompanyUser.UserId == userId && goalExtraMile.IsActive);

            Query.Select(goalExtraMile => new GoalExtraMile()
            {
                Id = goalExtraMile.Id,
                ExtraMileTargetEndDate = goalExtraMile.ExtraMileTargetEndDate.AddHours(timezone),
                ActualEndDate = goalExtraMile.ActualEndDate.Value.AddHours(timezone),
                GoalId = goalExtraMile.GoalId,
                Goal = goalExtraMile.Goal,
                IsVoided = goalExtraMile.IsVoided,
                IsActive = goalExtraMile.IsActive,
                GoalStatusId = goalExtraMile.GoalStatusId,
                GoalStatus = goalExtraMile.GoalStatus
            });

            if (goalId > 0)
            {
                Query.Where(goalExtraMile => goalExtraMile.GoalId == goalId);
            }

            if (!string.IsNullOrEmpty(description))
            {
                Query.Search(goalExtraMile => goalExtraMile.Goal.Description.ToLower(), "%" + description.ToLower() + "%");
            }

            if (dateStartFrom is not null && dateStartTo is not null)
            {
                Query.Where(goalExtraMile => dateStartFrom >= goalExtraMile.Goal.StartDate && dateStartTo <= goalExtraMile.Goal.StartDate);
            }

            if (actualendDateFrom is not null && actualendDateTo is not null)
            {
                Query.Where(goalExtraMile => actualendDateFrom >= goalExtraMile.ActualEndDate && actualendDateTo <= goalExtraMile.ActualEndDate);
            }

            if (projectId > 0)
            {
                Query.Where(goalExtraMile => goalExtraMile.Goal.ProjectId == projectId);
            }

            if (storyPoints > 0)
            {
                Query.Where(goalExtraMile => goalExtraMile.Goal.StoryPoint.Points == storyPoints);
            }

            if (scrumMasterId is not null)
            {
                Query.Where(goalExtraMile => goalExtraMile.Goal.ScrumMasterId.Contains(scrumMasterId));
            }

            if (jiraTicketId is not null)
            {
                Query.Where(goalExtraMile => goalExtraMile.Goal.JiraTicketId.Contains(jiraTicketId));
            }

            if (priorityLevel is not null)
            {
                Query.Where(goalExtraMile => goalExtraMile.Goal.PriorityLevel.Level.Contains(priorityLevel));
            }

            if (goalExtraMileStatusId > 0)
            {
                Query.Where(goalExtraMile => goalExtraMile.GoalStatusId == goalExtraMileStatusId);
            }

            if (paginate)
            {
                perPage = pageNumber == 0 ? 0 : perPage;
                Query.Skip((pageNumber - 1) * perPage).Take(perPage);
            }
        }
    }
}
