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

public class UpdateTaskAsync_Test
{
    /// <summary>
    /// Check result when in the proccess task is null
    /// </summary>
    /// <returns>Error Task Not Found<</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskNotExist_ReturnErrorTaskNotFound()
    {
        //Declaration Variables
        User user = new();
        NetForemost.Core.Entities.Tasks.Task task = new();

        //Create Simulated test
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out _,
            out _,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration Test
        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync((NetForemost.Core.Entities.Tasks.Task)null);

        //Validation Test
        var result = await taskService.UpdateTaskAsync(task, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskNotFound.Replace("[id]", task.Id.ToString()),
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check result in the proccess when tasktype not belong to the project
    /// </summary>
    /// <returns>ErrorT Task Type Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeNotBelongtoProject_ReturnErrorTaskTypeNotFound()
    {
        //Declaration Variables
        User user = new();
        NetForemost.Core.Entities.Tasks.Task task = new() { TypeId = 1 };
        NetForemost.Core.Entities.Tasks.Task task2 = new() { TypeId = 2 };

        //Create Simulated test
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out Mock<ITaskTypeService> taskTypeService,
            out _,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository_);

        //Configuration Test
        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(task2);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateTaskTypeBelongsToProjectAsync(
            It.IsAny<int>(),
            It.IsAny<int>())).ReturnsAsync(
            Result<TaskType>.Invalid(new() { new() { ErrorMessage = ErrorStrings.TaskTypeNotFound } }));

        //Validation Test
        var result = await taskService.UpdateTaskAsync(task, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.TaskTypeNotFound,
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check result when User not have access for the tasktype
    /// </summary>
    /// <returns>Error Company Not Found</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenTaskTypeNotAccessForUsert_ReturnErrorCompanyNotFound()
    {
        //Declaration Variables
        User user = new();
        TaskType taskType = new();
        NetForemost.Core.Entities.Tasks.Task task = new() { TypeId = 1 };
        NetForemost.Core.Entities.Tasks.Task task2 = new() { TypeId = 2 };
        var project = new Project() { Id = 1 };
        var company = new Company { Id = 1 };
        var companyUser = new CompanyUser() { Id = 1, User = user, UserId = user.Id, Company = company, CompanyId = company.Id };
        var owner = new ProjectCompanyUser() { Id = 1, CompanyUser = companyUser, CompanyUserId = company.Id, Project = project, ProjectId = project.Id };
        Goal goal = new() { Id = 1, Owner = owner, OwnerId = owner.Id };

        //Create Simulated test
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out Mock<ITaskTypeService> taskTypeService,
            out _,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration Test
        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(task2);

        goalRepository.Setup(goalRepository => goalRepository.FirstOrDefaultAsync(
            It.IsAny<GetGoalWithRelationDataSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateTaskTypeBelongsToProjectAsync(
            It.IsAny<int>(),
            It.IsAny<int>())).ReturnsAsync(
            Result<TaskType>.Success(taskType));

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<TaskType>.Invalid(
                new() { new() { ErrorMessage = ErrorStrings.CompanyNotFound } }));

        //Validation Test
        var result = await taskService.UpdateTaskAsync(task, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.CompanyNotFound,
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check result when goal not belong to user
    /// </summary>
    /// <returns> Error Goal Not Found< </returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenGoalNotBelongToUser_ReturnErrorGoalNotFound()
    {
        //Declaration Variables
        User user = new();
        TaskType taskType = new();
        NetForemost.Core.Entities.Tasks.Task task = new() { TypeId = 1, GoalId = 1 };
        NetForemost.Core.Entities.Tasks.Task task2 = new() { TypeId = 2, GoalId = 2 };
        var project = new Project() { Id = 1 };
        var company = new Company { Id = 1 };
        var companyUser = new CompanyUser() { Id = 1, User = user, UserId = user.Id, Company = company, CompanyId = company.Id };
        var owner = new ProjectCompanyUser() { Id = 1, CompanyUser = companyUser, CompanyUserId = company.Id, Project = project, ProjectId = project.Id };
        Goal goal = new() { Id = 1, Owner = owner, OwnerId = owner.Id };

        //Create Simulated test
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration Test
        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(task2);

        goalRepository.Setup(goalRepository => goalRepository.FirstOrDefaultAsync(
            It.IsAny<GetGoalWithRelationDataSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateTaskTypeBelongsToProjectAsync(
            It.IsAny<int>(),
            It.IsAny<int>())).ReturnsAsync(
            Result<TaskType>.Success(taskType));

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<TaskType>.Success(taskType));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToUser(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<Goal>.Invalid(new() { new() { ErrorMessage = ErrorStrings.GoalNotFound } }));

        //Validation Test
        var result = await taskService.UpdateTaskAsync(task, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.GoalNotFound,
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check result when goal not belong to project
    /// </summary>
    /// <returns>< error Goal Not Owned By Project </returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenGoalNotBelongToPoject_ReturnErrorGoalNotOwnedByProject()
    {
        //Declaration Variables
        User user = new();
        TaskType taskType = new();
        NetForemost.Core.Entities.Tasks.Task task = new() { TypeId = 1, GoalId = 1 };
        NetForemost.Core.Entities.Tasks.Task task2 = new() { TypeId = 2, GoalId = 2 };
        var project = new Project() { Id = 1 };
        var company = new Company { Id = 1 };
        var companyUser = new CompanyUser() { Id = 1, User = user, UserId = user.Id, Company = company, CompanyId = company.Id };
        var owner = new ProjectCompanyUser() { Id = 1, CompanyUser = companyUser, CompanyUserId = company.Id, Project = project, ProjectId = project.Id };
        Goal goal = new() { Id = 1, Owner = owner, OwnerId = owner.Id };

        //Create Simulated test
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration Test
        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(task2);

        goalRepository.Setup(goalRepository => goalRepository.FirstOrDefaultAsync(
            It.IsAny<GetGoalWithRelationDataSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateTaskTypeBelongsToProjectAsync(
            It.IsAny<int>(),
            It.IsAny<int>())).ReturnsAsync(
            Result<TaskType>.Success(taskType));

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<TaskType>.Success(taskType));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToUser(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<Goal>.Success(goal));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToProject(
            It.IsAny<Goal>(),
            It.IsAny<int>())).Returns(
            Result<Goal>.Invalid(new() { new() { ErrorMessage = ErrorStrings.GoalNotOwnedByProject } }));


        //Validation Test
        var result = await taskService.UpdateTaskAsync(task, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorStrings.GoalNotOwnedByProject,
            ErrorHelper.GetValidationErrors(result.ValidationErrors));
    }

    /// <summary>
    /// Check result when UnexpectedError
    /// </summary>
    /// <returns><UnexpectedError , TestError </returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenUnExpectedError_ReturnErrorTestError()
    {
        //Declaration Variables
        User user = new();
        TaskType taskType = new();
        Goal goal = new();
        NetForemost.Core.Entities.Tasks.Task task = new() { TypeId = 1, GoalId = 1 };
        NetForemost.Core.Entities.Tasks.Task task2 = new() { TypeId = 2, GoalId = 2 };
        var errorMessage = "Test Error";

        //Create Simulated test
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration Test
        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).Throws(new Exception(errorMessage));

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateTaskTypeBelongsToProjectAsync(
            It.IsAny<int>(),
            It.IsAny<int>())).Throws(new Exception(errorMessage));

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
        var result = await taskService.UpdateTaskAsync(task, user.Id);

        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), errorMessage);
    }

    /// <summary>
    /// Check result when all is success
    /// </summary>
    /// <returns><IsSuccess</returns>
    [Fact]
    public async System.Threading.Tasks.Task WhenAllIsCorrect_ReturnSuccess()
    {
        //Declaration Variables
        User user = new();
        TaskType taskType = new();
        var project = new Project() { Id = 1 };
        var company = new Company { Id = 1 };
        var companyUser = new CompanyUser() { Id = 1, User = user, UserId = user.Id, Company = company, CompanyId = company.Id };
        var owner = new ProjectCompanyUser() { Id = 1, CompanyUser = companyUser, CompanyUserId = company.Id, Project = project, ProjectId = project.Id };
        NetForemost.Core.Entities.Tasks.Task task = new() { TypeId = 1, GoalId = 1 };
        NetForemost.Core.Entities.Tasks.Task task2 = new() { TypeId = 2, GoalId = 2 };
        Goal goal = new() { Id = 1, Owner = owner, OwnerId = owner.Id };

        //Create Simulated test
        var taskService = ServiceUtilities.TaskService(
            out Mock<IAsyncRepository<NetForemost.Core.Entities.Tasks.Task>> taskRepository,
            out _,
            out Mock<ITaskTypeService> taskTypeService,
            out Mock<IGoalService> goalService,
            out Mock<UserManager<User>> userManager,
            out Mock<IAsyncRepository<Goal>> goalRepository);

        //Configuration Test
        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(task2);

        goalRepository.Setup(goalRepository => goalRepository.FirstOrDefaultAsync(
            It.IsAny<GetGoalWithRelationDataSpecification>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(goal);

        userManager.Setup(userManager => userManager.FindByIdAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateTaskTypeBelongsToProjectAsync(
            It.IsAny<int>(),
            It.IsAny<int>())).ReturnsAsync(
            Result<TaskType>.Success(taskType));

        taskTypeService.Setup(taskTypeService => taskTypeService.ValidateAccessTaskTypeAsync(
            It.IsAny<TaskType>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<TaskType>.Success(taskType));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToUser(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(
            Result<Goal>.Success(goal));

        goalService.Setup(goalService => goalService.ValidateActiveGoalBelongsToProject(
            It.IsAny<Goal>(),
            It.IsAny<int>())).Returns(
            Result<Goal>.Success(goal));

        //Validation Test
        var result = await taskService.UpdateTaskAsync(task, user.Id);

        Assert.True(result.IsSuccess);
    }
}
