namespace NetForemost.Core.Entities.Goals;

public class FindAllGoalsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime TargetEndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public bool HasExtraMileGoal { get; set; }
    public bool Approved { get; set; }
    public string Description { get; set; }
    public double EstimatedHours { get; set; }
    public int TimeSpentInSecond { get; set; }


    //entities
    public GetStoryPointDto? StoryPoint { get; set; }
    public GetPriorityLevelDto? PriorityLevel { get; set; }
    public GetProjectDto? Project { get; set; }
    public GetUserDto? ScrumMaster { get; set; }
    public GetGoalStatusDto? GoalStatus { get; set; }
}

public class GetStoryPointDto
{
    public int Id { get; set; }
    public int? Points { get; set; }
}

public class GetPriorityLevelDto
{
    public int Id { get; set; }
    public string? Level { get; set; }
}

public class GetProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class GetUserDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class GetGoalStatusDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

