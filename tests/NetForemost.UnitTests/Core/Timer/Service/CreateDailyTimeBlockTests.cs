using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;
using TaskEntity = NetForemost.Core.Entities.Tasks.Task;

namespace NetForemost.UnitTests.Core.Timer.Service;
public class CreateDailyTimeBlockTests
{

    [Fact]
    public async Task CreateDailyTimeBlock_WhenTaskNotOwnedByUser_ReturnsError()
    {
        // Arrange
        User user = new User
        {
            Id = "testUserId"
        };
        var task = new TaskEntity() { Id = 1 };
        var DailyTimeBlock = new DailyTimeBlock { TaskId = task.Id, Task = task };
        var userRepository = new Mock<UserManager<User>>();

        var timerService = ServiceUtilities.CreateTimerService(
            out Mock<IAsyncRepository<TaskEntity>> taskRepository,
            out _,
            out _,
            userRepository
            );

        userRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(task);

        // Act
        var result = await timerService.CreateDailyTimeBlock(user, DailyTimeBlock);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.TaskNotOwnToUser, result.ValidationErrors[0].ErrorMessage);
    }

    [Fact]
    public async Task CreateDailyTimeBlock_WhenInvalidOrder_ReturnsError()
    {
        // Arrange
        User user = new User
        {
            Id = "testUserId"
        };
        var task = new TaskEntity() { Id = 1 };
        var DailyTimeBlock = new DailyTimeBlock { TimeStart = DateTime.MinValue, TaskId = task.Id, Task = task };
        {
        };
        var taskEntity = new TaskEntity();
        DailyTimeBlock.Task = taskEntity;
        var userRepository = new Mock<UserManager<User>>();

        userRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

        var timeTrackingService = ServiceUtilities.CreateTimerService(
            out Mock<IAsyncRepository<TaskEntity>> taskRepository,
            out _,
            out _,
            userRepository
            );

        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(task);

        taskRepository.Setup(taskRepository => taskRepository.FirstOrDefaultAsync(
            It.IsAny<GetTaskByUserId>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(task);

        // Act
        var result = await timeTrackingService.CreateDailyTimeBlock(user, DailyTimeBlock);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.TimeStartIsGreaterThanTimeEnd, result.ValidationErrors[0].ErrorMessage);
    }

    [Fact]
    public async Task CreateDailyTimeBlock_WhenExceptionOccurs_ReturnsError()
    {
        // Arrange
        User user = new User
        {
            Id = "testUserId"
        };
        var DailyTimeBlock = new DailyTimeBlock { };
        var userRepository = new Mock<UserManager<User>>();

        var timerService = ServiceUtilities.CreateTimerService(
            out Mock<IAsyncRepository<TaskEntity>> taskRepository,
            out _,
            out _,
            userRepository
            );

        userRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

        taskRepository.Setup(taskRepository => taskRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).Throws(new Exception("Test exception"));

        // Act
        var result = await timerService.CreateDailyTimeBlock(user, DailyTimeBlock);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Test exception", result.Errors);
    }
}