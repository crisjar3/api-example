﻿using Microsoft.AspNetCore.Identity;
using Moq;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Interfaces.Email;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Account.Services;

public class GenerateConfirmEmailTokenAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Generate Confirm Email Token
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenConfirmEmailTokenIsCorrect_ReturnSuccess()
    {
        //Delcarations of variables
        var tokenTest = "Is a test Token";
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = false
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.GenerateConfirmEmailTokenAsync(user.Email);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the email not exist and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Email Not Found </returns>
    [Fact]
    public async Task WhenEmailNotExist_ReturnsEmailNotFound()
    {
        //Delcarations of variables
        var tokenTest = "Is a test Token";
        var emailRegistered = "test_registered@email.com";
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test_not_registered@email.com",
            EmailConfirmed = false
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            emailRegistered
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.GenerateConfirmEmailTokenAsync(user.Email);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_EmailNotFound);
    }

    /// <summary>
    /// It checks if the email is confirmed and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Returns Email has already been confirmed </returns>
    [Fact]
    public async Task WhenUserEmailIsConfirmed_ReturnsEmailHasAlreadyBeenConfirmed()
    {
        //Delcarations of variables
        var tokenTest = "Is a test Token";
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = true
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).ReturnsAsync(user);

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).ReturnsAsync(tokenTest);

        userManager.Setup(userManager => userManager.IsEmailConfirmedAsync(
            It.IsAny<User>()
            )).ReturnsAsync(true);

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).ReturnsAsync(It.IsAny<bool>());

        var result = await accountService.GenerateConfirmEmailTokenAsync(user.Email);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.Result_VerifiedAccount);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccursToGenerateConfirmEmailToken_ReturnsError()
    {
        //Delcarations of variables
        var user = new User()
        {
            UserName = "username",
            PasswordHash = "oldPassword",
            Email = "test@email.com",
            EmailConfirmed = false
        };

        //Create the simulated service
        var accountService = ServiceUtilities.CreateHireUserService(
            out _, out _, out _, out _, out _, out _, out _,
            out Mock<IEmailService> emailService,
            out Mock<UserManager<User>> userManager, out _
            );

        //Configurations for tests
        userManager.Setup(userManager => userManager.FindByEmailAsync(
            It.IsAny<string>()
            )).Throws(new Exception());

        userManager.Setup(userManager => userManager.GenerateEmailConfirmationTokenAsync(
            It.IsAny<User>()
            )).Throws(new Exception());

        emailService.Setup(emailService => emailService.TrySendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>(),
            It.IsAny<string>()
            )).Throws(new Exception());

        var result = await accountService.GenerateConfirmEmailTokenAsync(user.Email);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}
