using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.BlockTimeTrackings;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;


namespace NetForemost.UnitTests.Core.Timer.Service;
public class CreateDailyMonitoringBlockTests
{
    [Fact]
    public async Task CreateDailyMonitoringBlock_WhenBlockTimeTrackingNotExist_ReturnsError()
    {
        // Arrange
        var userId = "testUserId";
        var dailyMonitoringBlock = new DailyMonitoringBlock();
        var userRepository = new Mock<UserManager<User>>();

        var timerService = ServiceUtilities.CreateTimerService(
            out _,
            out Mock<IAsyncRepository<DailyTimeBlock>> dailyTimeBlockRepository,
            out _,
            userRepository
            );

        userRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

        dailyTimeBlockRepository.Setup(repo =>
        repo.FirstOrDefaultAsync(
                It.IsAny<GetDailyMonitoringBlockByIdAndOwnerId>(),
                CancellationToken.None))
            .ReturnsAsync((DailyTimeBlock)null);

        // Act
        var result = await timerService.CreateDailyMonitoringBlock(dailyMonitoringBlock, userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorStrings.BlockTimeTrackingNotExist, result.ValidationErrors[0].ErrorMessage);
    }

    [Fact]
    public async Task CreateDailyMonitoringBlock_WhenBlockTimeTrackingExists_CreatesAndReturnsDailyMonitoringBlock()
    {
        // Arrange
        var userId = "testUserId";
        var dailyMonitoringBlock = new DailyMonitoringBlock();
        var dailyTimeBlockId = 123;

        var dailyTimeBlock = new DailyTimeBlock { Id = dailyTimeBlockId };
        var userRepository = new Mock<UserManager<User>>();

        var timerService = ServiceUtilities.CreateTimerService(
            out _,
            out Mock<IAsyncRepository<DailyTimeBlock>> dailyMonitoringBlockRepository,
            out _,
            userRepository
            );

        userRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

        dailyMonitoringBlockRepository.Setup(repo =>
        repo.FirstOrDefaultAsync(
                It.IsAny<GetDailyMonitoringBlockByIdAndOwnerId>(),
                new CancellationToken()))
            .ReturnsAsync(dailyTimeBlock);

        // Act
        var result = await timerService.CreateDailyMonitoringBlock(dailyMonitoringBlock, userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dailyMonitoringBlock, result.Value);
    }

    [Fact]
    public async Task CreateDailyMonitoringBlock_WhenExceptionOccurs_ReturnsError()
    {
        // Arrange
        var userId = "testUserId";
        var dailyMonitoringBlock = new DailyMonitoringBlock();
        var userRepository = new Mock<UserManager<User>>();

        var timeTrackingService = ServiceUtilities.CreateTimerService(
            out _,
            out Mock<IAsyncRepository<DailyTimeBlock>> blockTimeTrackingRepository,
            out _,
            userRepository
            );

        userRepository.Setup(repo => repo.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());

        blockTimeTrackingRepository.Setup(repo =>
        repo.FirstOrDefaultAsync(
                It.IsAny<GetDailyMonitoringBlockByIdAndOwnerId>(),
                CancellationToken.None))
            .Throws(new Exception("Test exception"));

        // Act
        var result = await timeTrackingService.CreateDailyMonitoringBlock(dailyMonitoringBlock, userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Test exception", result.Errors);
    }
}
