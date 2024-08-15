using Ardalis.Result;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Interfaces.Settings
{
    public interface IUserSettingsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userSettings"></param>
        /// <returns></returns>
        Task<Result<UserSettings>> CreateUserSettingsAsync(UserSettings userSettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userSettings"></param>
        /// <returns></returns>
        Task<Result<UserSettings>> UpdateUserSettingsAsync(UserSettings userSettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<Result<UserSettings>> GetUserSettingsAsync(int id);

        /// <summary>
        /// automatically creates a user setting by default for a new user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result<UserSettings>> CreateUserSettingsAutomaticAsync(string userId);
    }
}