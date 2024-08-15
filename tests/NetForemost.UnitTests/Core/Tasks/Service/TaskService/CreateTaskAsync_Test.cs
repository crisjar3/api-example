using Ardalis.Result;
using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Projects;
using NetForemost.Core.Entities.Tasks;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Goals;
using NetForemost.Core.Interfaces.Tasks;
using NetForemost.Core.Specifications.Goals;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Tasks.Service.TaskService;

public class CreateTaskAsync_Test
{
    /// <summary>
    /// Check proccess when taskType not is valid
    /// </summary>
    /// <returns> return ,Task Type Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTipeNotValidated_ReturnErrorTaskTypeNotFound()
    {
        //Declaration Variable
        var taskType = new TaskType() { Id = 1 };
        NetForemost.Core.Entities.Tasks.Task task = new() { Id = 1, TypeId = 1 };
        User user = new();

        //Created simulated service
        var taskService = ServiceUtilities.TaskService(
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
            out _, out _,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((TaskType)null);

        //Validation Test
        var result = await taskService.CreateTaskAsync(user.Id, "desc", DateTime.Now, 1, 1);

        Assert.Equal(ErrorStrings.TaskTypeNotFound.Replace("[id]", task.TypeId.ToString()),
                        ErrorHelper.GetValidationErrors(result.ValidationErrors));
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// Check proccess when user not access to taskType
    /// </summary>
    /// <returns> return error company not found </returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUserNotAccessToTaskType_ReturnErrorCompanyNotFound()
    {
        //Declaration Variable
        TaskType taskType = new() { Id = 1, CompanyId = 0, ProjectId = 0 };
        NetForemost.Core.Entities.Tasks.Task task = new() { CreatedBy = null, Type = taskType, TypeId = 1 };
        User user = new() { Id = "1" };
        Project project = new();
        var company = new Company() { Id = 1 };
        var companyUser = new CompanyUser() { Id = 1, User = user, UserId = user.Id, Company = company, CompanyId = company.Id };
        var owner = new ProjectCompanyUser() { Id = 1, Project = project, ProjectId = project.Id, CompanyUser = companyUser, CompanyUserId = companyUser.Id };
        Goal goal = new() { Id = 0, Owner = owner, OwnerId = owner.Id, ActualEndDate = DateTime.Parse("01/01/2100") };

        //Created simulated service
        var taskService = ServiceUtilities.TaskService(
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository
            );

        //Configuration test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(taskType);

        goalRepository.Setup(goalRepository => goalRepository.FirstOrDefaultAsync(
            It.IsAny<GetGoalWithRelationDataSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToUser(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(Result.Success(goal));

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).ReturnsAsync(Result<TaskType>.Invalid(new() { new() { ErrorMessage = ErrorStrings.CompanyNotFound } }));

        //Validation Test
        var result = await taskService.CreateTaskAsync(user.Id, "desc", DateTime.Now, 1, 1);

        Assert.Equal(ErrorStrings.CompanyNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// Check when goal does not belong to the User
    /// </summary>
    /// <returns>
    /// error , Goal Not Found
    /// </returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenGoalNotBelongToUser_ReturnErrorGoalNotFound()
    {
        //Declaration Variable
        Project project = new();
        User user = new() { Id = "1" };
        TaskType taskType = new() { Id = 1, CompanyId = 1, ProjectId = 1 };
        Goal goal = new() { Id = 1, Owner = new ProjectCompanyUser() { Id = 1 }, OwnerId = 1, ActualEndDate = DateTime.Parse("01/01/2100") };
        NetForemost.Core.Entities.Tasks.Task task = new()
        {
            CreatedBy = "Owner",
            Type = taskType,
            TypeId = 1,
            Goal = goal,
            GoalId = 1,
            Project = project,
            ProjectId = 1
        };

        //Created simulated service
        var taskService = ServiceUtilities.TaskService(
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService, out _, out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(taskType);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).ReturnsAsync(Result<TaskType>.Success(taskType));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToUser(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<Goal>.Invalid(new() { new() { ErrorMessage = ErrorStrings.GoalNotFound } }));

        //Validation Test
        var result = await taskService.CreateTaskAsync(user.Id, "desc", DateTime.Now, 1, 1);

        Assert.Equal(ErrorStrings.GoalNotFound, ErrorHelper.GetValidationErrors(result.ValidationErrors));
        Assert.False(result.IsSuccess);
    }

    /// <summary>
    /// Chenck When all proccess is correct
    /// </summary>
    /// <returns>IsSuccess</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnSuccess()
    {
        //Declaration Variable
        User user = new() { Id = "1" };
        Project project = new();
        var company = new Company() { Id = 1 };
        var companyUser = new CompanyUser() { Id = 1, User = user, UserId = user.Id, Company = company, CompanyId = company.Id };
        var owner = new ProjectCompanyUser() { Id = 1, Project = project, ProjectId = project.Id, CompanyUser = companyUser, CompanyUserId = companyUser.Id };

        TaskType taskType = new() { Id = 1, CompanyId = 1, ProjectId = 1 };
        Goal goal = new() { Id = 0, Owner = owner, OwnerId = owner.Id, ActualEndDate = DateTime.Parse("01/01/2100") };
        NetForemost.Core.Entities.Tasks.Task task = new()
        {
            CreatedBy = "Owner",
            Type = taskType,
            TypeId = 1,
            Goal = goal,
            GoalId = 1,
            Project = project,
            ProjectId = 1
        };

        //Created simulated service
        var taskService = ServiceUtilities.TaskService(
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository
            );

        //Configuration test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(taskType);

        goalRepository.Setup(goalRepository => goalRepository.FirstOrDefaultAsync(
            It.IsAny<GetGoalWithRelationDataSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).ReturnsAsync(Result<TaskType>.Success(taskType));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToUser(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(Result.Success(goal));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToProject(
            It.IsAny<Goal>(),
            It.IsAny<int>())).Returns(Result<Goal>.Success(goal));

        //Validation Test
        var result = await taskService.CreateTaskAsync(user.Id, "desc", DateTime.Now, 1, 1);

        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Chenck When proccess have unexpected error
    /// </summary>
    /// <returns>TestError</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnexpectedError_ReturnErrorTestError()
    {
        //Declaration Variable
        var errorMessage = "Test Error";
        Project project = new();
        User user = new() { Id = "1" };
        TaskType taskType = new() { Id = 1, CompanyId = 1, ProjectId = 1 };
        Goal goal = new() { Id = 0, Owner = new ProjectCompanyUser() { Id = 1 }, OwnerId = 1, ActualEndDate = DateTime.Parse("01/01/2100") };
        NetForemost.Core.Entities.Tasks.Task task = new()
        {
            CreatedBy = "Owner",
            Type = taskType,
            TypeId = 1,
            Goal = goal,
            GoalId = 1,
            Project = project,
            ProjectId = 1
        };

        //Created simulated service
        var taskService = ServiceUtilities.TaskService(
            out _,
            out Mock<IAsyncRepository<TaskType>> taskTypeRepository,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService, out _, out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration test
        taskTypeRepository.Setup(taskTypeRepository => taskTypeRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).Throws(new Exception(errorMessage));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToUser(
            It.IsAny<int>(),
            It.IsAny<string>())).Throws(new Exception(errorMessage));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToProject(
            It.IsAny<Goal>(),
            It.IsAny<int>())).Throws(new Exception(errorMessage));

        //Validation Test
        var result = await taskService.CreateTaskAsync(user.Id, "desc", DateTime.Now, 1, 1);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }
}
