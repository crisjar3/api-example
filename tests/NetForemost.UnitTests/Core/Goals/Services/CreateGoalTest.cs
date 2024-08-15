using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.PriorityLevels;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.StoryPoints;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Projects;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Goals.Services;

public class CreateGoalTest
{


    ///<summary>
    ///Verify the correct funtioning of the entire proccess
    ///</summary>
    ///<returs>Is success</returs>
    [Fact]
    public async Task WhenGoalAllVerified_ReturnSuccess()
    {
        //Declaration of Variables
        var company = new Company() { Id = 1 };
        var project = new Project() { Id = 1, CompanyId = company.Id, Company = company };
        Goal goal = new() { GoalStatusId = 1, EstimatedHours = 1, ProjectId = project.Id, Project = project };
        string userId = "userManager";
        var scrumMaster = new User();
        var goalStatus = new GoalStatus() { Id = 1, Name = "Status" };
        var priorityLevel = new PriorityLevel() { Id = 1, Description = "Level" };
        var storyPoint = new StoryPoint() { Id = 1 };

        var priorityLevelReposiotry = new Mock<IAsyncRepository<PriorityLevel>>();
        var storyPointRepository = new Mock<IAsyncRepository<StoryPoint>>();

        //Create simulated See
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMile,
            out Mock<IAsyncRepository<Project>> projectRepositoy,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository,
             priorityLevelReposiotry,
             storyPointRepository
            );

        //Configuration For test
        goalRepository.Setup(
            goalRepository => goalRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(goal);

        projectRepositoy.Setup(projectRepositoy
           => projectRepositoy.GetByIdAsync(
               It.IsAny<int>(),
               It.IsAny<CancellationToken>())
           ).ReturnsAsync(project);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetProjectByUserId>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ProjectCompanyUser());

        userManager.Setup(userManager =>
        userManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(scrumMaster);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(goalStatus);

        projectRepositoy.Setup(projectRepositoy
            => projectRepositoy.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(project);

        priorityLevelReposiotry.Setup(repo =>
        repo.GetByIdAsync(It.IsAny<int>(), new CancellationToken()))
            .ReturnsAsync(priorityLevel);

        storyPointRepository.Setup(repo =>
        repo.GetByIdAsync(It.IsAny<int>(), new CancellationToken()))
            .ReturnsAsync(storyPoint);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.AnyAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(true);

        goalRepository.Setup(
            goalRepository => goalRepository.AddAsync(
                It.IsAny<Goal>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(goal);

        var result = await goalService.CreateGoal(goal, userId, -6);

        //Validation For Test
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Verifi if project exist
    /// </summary>
    /// <returns>error ,project Not Found</returns>
    [Fact]
    public async Task WhenProjectIsNull_ReturnError()
    {
        //Declaration of Variables
        Goal goal = new Goal() { Project = new Project() { Id = 1 }, ProjectId = 1 };
        string userId = "userManager";
        User scrumMaster = new User();

        //Create simulated Service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMile,
            out Mock<IAsyncRepository<Project>> projectRepositoy,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository, null, null
            );

        //Configuration For test
        goalRepository.Setup(
        goalRepository => goalRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(goal);

        userManager.Setup(userManager =>
        userManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(scrumMaster);

        projectRepositoy.Setup(projectRepositoy
            => projectRepositoy.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new Project());

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.FirstOrDefaultAsync(
            It.IsAny<GetProjectByUserId>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync((ProjectCompanyUser)null);

        var result = await goalService.CreateGoal(goal, userId, -6);
        bool errorMessageIsCorrect = result.ValidationErrors.Any(error => error.ErrorMessage == ErrorStrings.ProjectNotFound.Replace("[id]", goal.ProjectId.ToString()));

        //Validation for teste
        Assert.False(result.IsSuccess);
        Assert.True(errorMessageIsCorrect);
    }

    /// <summary>
    /// verify is stimated our is invalid
    /// </summary>
    /// <returns>error Estimated hour not valid</returns>
    [Fact]
    public async Task WhenEstimatedOurIsInvalid_ReturnError()
    {
        //Declaration of Variables
        var goal = new Goal() { EstimatedHours = 0, GoalStatusId = 1 };
        string userId = "userManager";
        var project = new Project() { };
        var scrumMaster = new User();
        var goalStatus = new GoalStatus() { Id = 1, Name = "Status" };

        //Create simulated Service
        var goalService = ServiceUtilities.CreateGoalService(
                out Mock<IAsyncRepository<Goal>> goalRepository,
                out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMile,
                out Mock<IAsyncRepository<Project>> projectRepositoy,
                out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
                out Mock<UserManager<User>> userManager,
                out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository, null, null);

        //Configuration For test
        goalRepository.Setup(
            goalRepository => goalRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetProjectByUserId>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ProjectCompanyUser());

        userManager.Setup(userManager =>
        userManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(scrumMaster);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.GetByIdAsync(
            It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(goalStatus);

        projectRepositoy.Setup(projectRepositoy
            => projectRepositoy.AnyAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(true);

        projectRepositoy.Setup(projectRepositoy
           => projectRepositoy.GetByIdAsync(
               It.IsAny<int>(),
               It.IsAny<CancellationToken>())
           ).ReturnsAsync(project);

        goalStatusRepository.Setup(goalStatusRepository => goalStatusRepository.AnyAsync(
           It.IsAny<int>(),
           It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await goalService.CreateGoal(goal, userId, -6);
        bool errorMessageIsCorrect = result.ValidationErrors.Any(error => error.ErrorMessage == ErrorStrings.EstimatedHourNotValid);

        //Validation for Teste
        Assert.False(result.IsSuccess);
        Assert.True(errorMessageIsCorrect);
    }

    /// <summary>
    /// verify is ScrumMaster is null
    /// </summary>
    /// <returns>error Scrum Master Not Found</returns>
    [Fact]
    public async Task WhenScrumMasterIsNull_ReturnError()
    {
        //Declaration of Variables
        Goal goal = new Goal();
        string userId = "userManager";
        Project project = new Project();

        //Create simulated Service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMile,
            out Mock<IAsyncRepository<Project>> projectRepositoy,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository, null, null);

        //Configuration For test
        goalRepository.Setup(
            goalRepository => goalRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.FirstOrDefaultAsync(
                It.IsAny<GetProjectByUserId>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ProjectCompanyUser());

        userManager.Setup(userManager =>
        userManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync((User)null);

        projectRepositoy.Setup(projectRepositoy
            => projectRepositoy.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(project);

        var result = await goalService.CreateGoal(goal, userId, -6);
        bool errorMessageIsCorrect = result.ValidationErrors.Any(error => error.ErrorMessage == ErrorStrings.ScrumMasterNotFound);

        //Validation for Teste
        Assert.False(result.IsSuccess);
        Assert.True(errorMessageIsCorrect);
    }

    /// <summary>
    /// Check if the start date is less than the end date
    /// </summary>
    /// <returns>Error Invalid Goal End Date LessThanStarDate</returns>
    [Fact]
    public async Task WhenTargetdayNotIsCorrect_ReturnError()
    {
        //Declaration of Variables
        var endDate = "1/5/2023 8:30:52 AM";
        DateTime targetEndDate = DateTime.Parse(endDate);
        var startDate = "1/6/2023 8:30:52 AM";
        DateTime starteDate1 = DateTime.Parse(startDate);
        Goal goal = new Goal() { TargetEndDate = targetEndDate, StartDate = starteDate1 };
        string userId = "userManager";
        Project project = new Project();
        User scrumMaster = new User();

        //Create simulated Service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMile,
            out Mock<IAsyncRepository<Project>> projectRepositoy,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<ProjectCompanyUser>> projectCompanyUserRepository, null, null);

        //Configuration For test
        goalRepository.Setup(
            goalRepository => goalRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(goal);

        projectCompanyUserRepository.Setup(projectCompanyUserRepository => projectCompanyUserRepository.FirstOrDefaultAsync(
            It.IsAny<GetProjectByUserId>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ProjectCompanyUser());

        userManager.Setup(userManager =>
        userManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(scrumMaster);

        projectRepositoy.Setup(projectRepositoy
            => projectRepositoy.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(project);

        var result = await goalService.CreateGoal(goal, userId, -6);
        bool errorMessageIsCorrect = result.ValidationErrors.Any(error => error.ErrorMessage == ErrorStrings.InvalidGoal_EndDateLessThanStarDate);

        //Validation for test
        Assert.False(result.IsSuccess);
        Assert.True(errorMessageIsCorrect);
    }

    /// <summary>
    /// Check if the start date is less than the end date
    /// </summary>
    /// <returns>Error Invalid Goal End Date Less Than Star Date</returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccurs_ReturnError()
    {
        //Declaration of Variables
        var endDate = "1/5/2023 8:30:52 AM";
        DateTime targetEndDate = DateTime.Parse(endDate);
        var startDate = "1/6/2023 8:30:52 AM";
        DateTime starteDate1 = DateTime.Parse(startDate);
        Goal goal = new Goal() { TargetEndDate = targetEndDate, StartDate = starteDate1 };
        string userId = "userManager";
        Project project = new Project();
        User scrumMaster = new User();
        var testError = "TEST ERROR";

        //Create simulated Service
        var goalService = ServiceUtilities.CreateGoalService(
            out Mock<IAsyncRepository<Goal>> goalRepository,
            out Mock<IAsyncRepository<GoalExtraMile>> goalExtraMile,
            out Mock<IAsyncRepository<Project>> projectRepositoy,
            out Mock<IAsyncRepository<GoalStatus>> goalStatusRepository,
            out Mock<UserManager<User>> userManager,
            out _, null, null);

        //Configuration For test
        goalRepository.Setup(
            goalRepository => goalRepository.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(goal);

        userManager.Setup(userManager =>
        userManager.FindByIdAsync(
            It.IsAny<string>())).ReturnsAsync(scrumMaster);

        projectRepositoy.Setup(projectRepositoy
            => projectRepositoy.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>())
            ).Throws(new Exception(testError));

        var result = await goalService.CreateGoal(goal, userId, -6);
        bool errorMessageIsCorrect = result.ValidationErrors.Any(error => error.ErrorMessage == ErrorStrings.InvalidGoal_EndDateLessThanStarDate);

        //Validation for test
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}