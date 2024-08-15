using Ardalis.Specification;
using NetForemost.Core.Entities.Goals;

namespace NetForemost.Core.Specifications.Goals
{
    public class FindAllGoalsSpecification : Specification<Goal, FindAllGoalsDto>
    {
        public FindAllGoalsSpecification(
            string userId,
            string description,
            double estimatedHours,
            int projectId,
            int storyPoints,
            DateTime? dateStartTo, DateTime? dateStartFrom,
            DateTime? actualendDateTo, DateTime? actualendDateFrom,
            string scrumMasterId,
            string jiraTicketId,
            string priorityLevel,
            int? goalStatusId,
            double timeZone,
            int pageNumber, int perPage,
            DateTime CreationDateTo, DateTime CreationDateFrom,
            int companyId,
            bool pagination = false

        )
        {
            Query.Include(goal => goal.Project);
            Query.Include(goal => goal.ScrumMaster);
            Query.Include(goal => goal.StoryPoint);
            Query.Include(goal => goal.PriorityLevel);
            Query.Include(goal => goal.GoalStatus);
            Query.Include(goal => goal.Owner).ThenInclude(owner => owner.CompanyUser).ThenInclude(companyUser => companyUser.User);

            Query.Where(goal => goal.Owner.CompanyUser.UserId == userId);

            if (CreationDateTo > DateTime.MinValue && CreationDateFrom > DateTime.MinValue)
            {
                Query.Where(goal => goal.CreatedAt.Date >= CreationDateFrom.Date && goal.CreatedAt.Date <= CreationDateTo.Date);
            }

            if (!string.IsNullOrEmpty(description))
            {
                Query.Search(goal => goal.Description.ToLower(), "%" + description.ToLower() + "%");
            }

            if (dateStartFrom is not null && dateStartTo is not null)
            {
                Query.Where(goal => dateStartFrom >= goal.StartDate && dateStartTo <= goal.StartDate);
            }

            if (actualendDateFrom is not null && actualendDateTo is not null)
            {
                Query.Where(goal => actualendDateFrom >= goal.ActualEndDate && actualendDateTo <= goal.ActualEndDate);
            }

            if (projectId > 0)
            {
                Query.Where(goal => goal.ProjectId == projectId);
            }

            if (storyPoints > 0)
            {
                Query.Where(goal => goal.StoryPoint.Points == storyPoints);
            }

            if (companyId > 0)
            {
                Query.Where(goal => goal.Project.CompanyId == companyId);
            }

            if (scrumMasterId is not null)
            {
                Query.Where(goal => goal.ScrumMasterId.Contains(scrumMasterId));
            }

            if (jiraTicketId is not null)
            {
                Query.Where(goal => goal.JiraTicketId.Contains(jiraTicketId));
            }

            if (priorityLevel is not null)
            {
                Query.Where(goal => goal.PriorityLevel.Level.Contains(priorityLevel));
            }

            if (goalStatusId > 0)
            {
                Query.Where(goal => goal.GoalStatusId == goalStatusId || goal.GoalExtraMile.GoalStatusId == goalStatusId);
            }

            if (estimatedHours > 0)
            {
                Query.Where(goal => goal.EstimatedHours == estimatedHours);
            }

            if (pagination)
            {
                perPage = pageNumber == 0 ? 0 : perPage;
                Query.Skip((pageNumber - 1) * perPage).Take(perPage);

                Query.Select(goal => new FindAllGoalsDto()
                {
                    Id = goal.Id,
                    Name = goal.Name,
                    StartDate = goal.StartDate.AddHours(timeZone),
                    TargetEndDate = goal.TargetEndDate.AddHours(timeZone),
                    ActualEndDate = goal.ActualEndDate,
                    HasExtraMileGoal = goal.HasExtraMileGoal,
                    Approved = goal.Approved,
                    Description = goal.Description,
                    EstimatedHours = goal.EstimatedHours,
                    StoryPoint = new() { Id = goal.StoryPoint.Id, Points = goal.StoryPoint.Points },
                    PriorityLevel = new() { Id = goal.PriorityLevel.Id, Level = goal.PriorityLevel.Level },
                    Project = new() { Id = goal.Project.Id, Name = goal.Project.Name, Description = goal.Project.Description },
                    ScrumMaster = new() { Id = goal.ScrumMaster.Id, FirstName = goal.ScrumMaster.FirstName, LastName = goal.ScrumMaster.LastName },
                    GoalStatus = goal.GoalStatusId == null ? null : new() { Id = goal.GoalStatus.Id, Name = goal.GoalStatus.Name },
                    TimeSpentInSecond = (int)Math.Round(goal.TimeSpentInSecond)
                });
            }
        }
    }
}
