using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Core.Entities.Users;
using NetForemost.SharedKernel.Entities;

namespace NetForemost.Core.Entities.Goals;

public class Goal : BaseEntity
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime TargetEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public bool HasExtraMileGoal { get; set; }
    public bool Approved { get; set; }
    public string Description { get; set; }
    public double EstimatedHours { get; set; }
    public int StoryPointId { get; set; }
    public virtual StoryPoint? StoryPoint { get; set; }
    public string? JiraTicketId { get; set; }
    public int PriorityLevelId { get; set; }
    public virtual PriorityLevel? PriorityLevel { get; set; }
    public int ProjectId { get; set; }
    public virtual Project? Project { get; set; }
    public string ScrumMasterId { get; set; }
    public User ScrumMaster { get; set; }
    public int? GoalStatusId { get; set; }
    public int? OwnerId { get; set; }

    public virtual ProjectCompanyUser? Owner { get; set; }
    public virtual GoalStatus? GoalStatus { get; set; }
    public virtual GoalExtraMile? GoalExtraMile { get; set; }
    public virtual ICollection<Entities.Tasks.Task> Tasks { get; set; }
    public double TimeSpentInSecond { get; set; }

    //Value Calculated
    private double TimeSpentInHours => TimeSpentInSecond / SecondPerHours;
    private double EstimatedWorkEfficiency => EstimatedHours * PerformanceExtraMilePercentage;

    //Consts required for calculations

    private const int SecondPerHours = 3600;
    private const double PerformanceExtraMilePercentage = 0.8;

    public void UpdateGoal(Goal newDataGoal, string userId)
    {
        // Verify if new information goal name and description is empty
        Name = newDataGoal.Name != Name && newDataGoal.Name is not null ? newDataGoal.Name : Name;
        Description = Description != newDataGoal.Description && newDataGoal.Description is not null ? newDataGoal.Description : Description;

        JiraTicketId = JiraTicketId != newDataGoal.JiraTicketId && newDataGoal.JiraTicketId is not null ? newDataGoal.JiraTicketId : JiraTicketId;
        ScrumMasterId = ScrumMasterId != newDataGoal.ScrumMasterId ? newDataGoal.ScrumMasterId : ScrumMasterId;
        ProjectId = ProjectId != newDataGoal.ProjectId ? newDataGoal.ProjectId : ProjectId;
        StoryPointId = StoryPointId != newDataGoal.StoryPointId ? newDataGoal.StoryPointId : StoryPointId;
        PriorityLevelId = PriorityLevelId != newDataGoal.PriorityLevelId ? newDataGoal.PriorityLevelId : PriorityLevelId;

        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }

    public void UpdateStausGoal(GoalStatus newGoalStatus, string userId)
    {

        if (newGoalStatus.IsFinalStatus)
        {
            ActualEndDate = DateTime.UtcNow;
        }

        //Goal updated from finished to other status like `Progress`
        if (GoalStatus.IsFinalStatus)
        {
            ActualEndDate = null;
        }

        //Goal had been marked as a Extra Mile
        if (HasExtraMileGoal)
        {
            CancellExtraMile();
        }

        if (IsValidasExtraMile())
        {
            SetGoalAsExtraMile();
        }

        GoalStatusId = newGoalStatus.Id;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }

    public void CancellExtraMile()
    {
        ActualEndDate = null;
        HasExtraMileGoal = false;
    }

    public void SetGoalAsExtraMile()
    {
        HasExtraMileGoal = true;
    }

    public bool IsValidasExtraMile()
    {
        if (ActualEndDate is null)
        {
            return false;
        }

        var finisheAheadSchedule = ActualEndDate < TargetEndDate;
        var lessTimeSpentThanExpected = EstimatedWorkEfficiency < TimeSpentInHours;

        return finisheAheadSchedule && lessTimeSpentThanExpected;
    }

    public void AddTime(DateTime timeStart, DateTime timeEnd)
    {
        TimeSpentInSecond += (timeEnd - timeStart).TotalSeconds;
    }
}