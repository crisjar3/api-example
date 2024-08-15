using Ardalis.Result;
using NetForemost.Core.Entities.Roles;
using NetForemost.Core.Entities.Users;

namespace NetForemost.Core.Interfaces.Account;

public interface IAccountService
{
    /// <summary>
    ///     Create a new application user.
    /// </summary>
    /// <param name="applicationUser">The application user to create.</param>
    /// <param name="password">The password for the user.</param>
    /// <returns>Returns the user of the application created</returns>
    Task<Result<User>> CreateHireUser(User applicationUser, string password);

    /// <summary>
    ///     Create a new application user of Talent Type.
    /// </summary>
    /// <param name="applicationUser">The application user to create.</param>
    /// <param name="password">The password for the user.</param>
    /// <returns></returns>
    Task<Result<User>> CreateTalentUserAsync(User user, string password);

    /// <summary>
    ///     Update the application user.
    /// </summary>
    /// <param name="userNewData"></param>
    /// <param name="currentPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    Task<Result<User>> UpdateUserAsync(User userNewData);

    /// <summary>
    ///     Confirm the email of a registered user.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="code">Code to confirm email.</param>
    /// <returns>Return a field of boolean type to verify if the process was completed successfully</returns>
    Task<Result<bool>> ConfirmEmailAsync(string userId, string code);

    /// <summary>
    ///     Change password user.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="oldPassword">The password for the user.</param>
    /// <param name="password">The password for the user.</param>
    /// <returns>Return a boolean value to verify if the process was completed successfully</returns>
    Task<Result<bool>> UpdatePasswordAsync(string username, string oldPassword, string password);

    /// <summary>
    ///     Generate a token to reset password.
    /// </summary>
    /// <param name="email">The email of user.</param>
    /// <returns>Return a boolean value to verify if the process was completed successfully</returns>
    Task<Result<bool>> GenerateResetPasswordTokenAsync(string email);

    /// <summary>
    ///     Reset password.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="code">The reset password token.</param>
    /// <param name="password">The password for the user.</param>
    /// <returns>Return a boolean value to verify if the process was completed successfully</returns>
    Task<Result<bool>> ResetPasswordAsync(string userId, string code, string password);

    /// <summary>
    ///     Generate a token to confirm email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Result<bool>> GenerateConfirmEmailTokenAsync(string email);

    /// <summary>
    ///     Get User by id.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>Return a the user specified</returns>
    Task<Result<User>> GetUserByIdAsync(string userId);

    /// <summary>
    /// Add image user by id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ImageUrl"></param>
    /// <returns></returns>
    Task<Result<bool>> AddImageToUserAsync(string userId, string ImageUrl);

    /// <summary>
    /// Verify if username and email exist
    /// </summary>
    /// <param name="email"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<Result<bool>> VarifyUserAsync(string email, string userName);

    /// <summary>
    /// Get all the roles that a user can have.
    /// </summary>
    /// <returns>Project created.</returns>
    Task<Result<List<Role>>> GetRolesAsync();
}