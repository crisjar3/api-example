using Ardalis.Result;
using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NetForemost.Core.Dtos.Timer;
using NetForemost.Core.Entities.Goals;
using NetForemost.Core.Entities.Timer;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Timer;
using NetForemost.Core.Specifications.BlockTimeTrackings;
using NetForemost.Core.Specifications.Tasks;
using NetForemost.Core.Specifications.Timer;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using TaskEntity = NetForemost.Core.Entities.Tasks.Task;

namespace NetForemost.Core.Services.TimeTrackings;
public class TimerService : ITimerService
{
    private readonly IAsyncRepository<TaskEntity> _taskRepository;
    private readonly IAsyncRepository<DailyTimeBlock> _dailyTimeBlockRepository;
    private readonly UserManager<User> _userManager;
    private readonly IAsyncRepository<Goal> _goalRepository;

    public TimerService(IAsyncRepository<TaskEntity> taskRepository, IAsyncRepository<DailyTimeBlock> dailyTimeBlockRepository, UserManager<User> userManager,IAsyncRepository<Goal> goalRepository)
    {
        _taskRepository = taskRepository;
        _dailyTimeBlockRepository = dailyTimeBlockRepository;
        _userManager = userManager;
        _goalRepository = goalRepository;
    }

    public async Task<Result<DailyMonitoringBlock>> CreateDailyMonitoringBlock(DailyMonitoringBlock dailyMonitoringBlock, string userId)
    {
        try
        {
            var dailyEntry = await _dailyTimeBlockRepository.FirstOrDefaultAsync(new GetDailyMonitoringBlockByIdAndOwnerId(dailyMonitoringBlock.DailyTimeBlockId, userId));

            if (dailyEntry is null)
            {
                return Result.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.BlockTimeTrackingNotExist
                        }
                    });
            }

            //set the owner
            dailyMonitoringBlock.SetOwner(userId);

            //add the block monitoring
            dailyEntry.AddMonitoringBlock(dailyMonitoringBlock);

            await _dailyTimeBlockRepository.SaveChangesAsync();

            return Result.Success(dailyMonitoringBlock);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<DailyTimeBlock>> CreateDailyTimeBlock(User user, DailyTimeBlock dailyTimeBlock)
    {
        try
        {
            var userId = user.Id;
            // Verify if task exist

            var task = await _taskRepository.GetByIdAsync(dailyTimeBlock.TaskId);

            if (task is null)
            {
                return Result.Invalid(new()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.TaskNotFound
                    }
                });
            }

            var taskToUpdate = await _taskRepository.FirstOrDefaultAsync(new GetTaskByUserId(userId, dailyTimeBlock.TaskId));
            
            if (taskToUpdate is null)
            {
                return Result.Invalid(new()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.TaskNotOwnToUser
                    }
                });
            }

            //validate if its have a correct order
            if (!dailyTimeBlock.IsTimeEndGreaterThanTimeStart())
            {
                return Result.Invalid(new()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.TimeStartIsGreaterThanTimeEnd
                    }
                });
            }

            //set default values
            dailyTimeBlock.SetOwner(userId);
            dailyTimeBlock.OwnerId = task.OwnerId;

            //get start of day and end, in UTC-0
            var dayInit = dailyTimeBlock.TimeStart.Date.AddHours(-user.TimeZone.Offset);
            var dayEnd = dayInit.AddHours(24);


            var previousTimeBlocks = await _dailyTimeBlockRepository.ListAsync(new GetTimeBlocksByDayAndUserIdSpecification(userId, dayInit, dayEnd));

            //convert to UTC-0
            dailyTimeBlock.ConvertDatesToTimezone(Convert.ToInt16(user.TimeZone.Offset));

            Goal? goalToUpdate = await _goalRepository.GetByIdAsync(task.GoalId);
            if (goalToUpdate is null)
            {
                return Result.Invalid(new()
                {
                    new()
                    {
                        ErrorMessage = ErrorStrings.GoalNotFound
                    }
                });
            }

            if (!previousTimeBlocks.IsNullOrEmpty())
            {
                DailyTimeBlock lastTimeBlock = previousTimeBlocks.OrderByDescending(block => block.TimeStart).FirstOrDefault();
                //Add new timeblock in last timeblock  if task id is equal and timeDiference beetween las time block date end is greatesr of new timeblock timestart
                if (dailyTimeBlock.TaskId == lastTimeBlock.TaskId && IsTimeDifferenceAcceptable(lastTimeBlock.TimeStart, dailyTimeBlock.TimeStart))
                {
                    goalToUpdate.AddTime(lastTimeBlock.TimeEnd, dailyTimeBlock.TimeEnd);
                    taskToUpdate.AddTime(lastTimeBlock.TimeEnd, dailyTimeBlock.TimeEnd);

                    lastTimeBlock.TimeEnd = dailyTimeBlock.TimeEnd;

                    await _dailyTimeBlockRepository.UpdateAsync(lastTimeBlock);

                    return Result.Success(lastTimeBlock);
                }

            }

            goalToUpdate.AddTime(dailyTimeBlock.TimeStart, dailyTimeBlock.TimeEnd);
            taskToUpdate.AddTime(dailyTimeBlock.TimeStart, dailyTimeBlock.TimeEnd);
            var newDailyTimeBlock = await _dailyTimeBlockRepository.AddAsync(dailyTimeBlock);

            //Update database
            await _taskRepository.UpdateAsync(taskToUpdate);
            await _goalRepository.UpdateAsync(goalToUpdate);

            //Add navegation entities
            dailyTimeBlock.Task = taskToUpdate;
            dailyTimeBlock.Task.Goal = goalToUpdate;

            return Result.Success(dailyTimeBlock);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    public async Task<Result<DailyEntryDto>> GetDailyEntryByDayAndUserId(string userId, DateTime date)
    {
        try
        {
            //Get user
            var user = await _userManager.FindByIdAsync(userId);
            //Transform day to UTC - X of user
            var dayInit = date.Date.AddHours(-user.TimeZone.Offset);
            var dayEnd = dayInit.AddHours(24);


            var timeBlocks = await _dailyTimeBlockRepository.ListAsync(new GetTimeBlocksByDayAndUserIdSpecification(userId, dayInit, dayEnd));

            if (timeBlocks is null)
            {
                return Result.NotFound("Daily Entry Not found");
            }

            var totalTraking = new double();

            timeBlocks.ForEach(block =>
            {
                block.TimeStart = (block.TimeStart < dayInit) ? dayInit : block.TimeStart;
                block.TimeEnd = (block.TimeEnd > dayEnd) ? dayEnd : block.TimeEnd;
                totalTraking += (block.TimeEnd - block.TimeStart).TotalSeconds;
            });

            DailyEntryDto dailyEntry = new()
            {
                TotalTrackingTime = totalTraking,
                KeystrokesMin = 0,
                OwnerId = userId,
                DateStart = date
            };

            return Result.Success(dailyEntry);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }

    //evaluate if the differece between a date and the last block is minor to 5 seg
    private bool IsTimeDifferenceAcceptable(DateTime dateEnd, DateTime dateStart)
    {
        const int DifferenceInSecondToAddNewBlock = 60;

        var difference = dateStart - dateEnd;

        return difference.TotalSeconds <= DifferenceInSecondToAddNewBlock;
    }
}