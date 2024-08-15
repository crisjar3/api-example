using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NetForemost.UnitTests.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace NetForemost.UnitTests.Core.Authentication.Services.TokenManagerService;

public class GetPrincipalFromExpiredTokenTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Get Principal From Expired Token
    /// </summary>
    /// <returns> Returns Success if all is correct </returns>
    [Fact]
    public async Task WhenGetPrincipalFromTokenIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var key = "Ym7AD3OT2kpuIRcVAXCweYhV64B0Oi9ETAO6XRbqB8LDL3tF4bMk9x/59PljcGbP5v38BSzCjD1VTwuO6iWA8uzDVAjw2fMNfcT2/LyRlMOsynblo3envlivtgHnKkZj6HqRrG5ltgwy5NsCQ7WwwYPkldhLTF+wUYAnq28+QnU=";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var tokenGenerate = new JwtSecurityToken(
            claims: new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, "test"),
                new Claim(JwtRegisteredClaimNames.Aud, "test"),
                new Claim(JwtRegisteredClaimNames.Sub, "1234567890"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            },

            expires: new DateTimeOffset(DateTime.Now.AddMinutes(1)).DateTime,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerate);

        // Create the simulated service
        var tokenManagerService = ServiceUtilities.CreateTokenManagerService(
            out _, out _, out _,
            out Mock<IConfiguration> configuration
            );

        // Configurations for tests
        configuration.Setup(configuration => configuration["Authentication:Jwt:SecretKey"]).Returns(key);

        var result = tokenManagerService.GetPrincipalFromExpiredToken(token);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Returns Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrosOccur_ReturnsError()
    {
        // Declarations of variables
        var key = "Ym7AD3OT2kpuIRcVAXCweYhV64B0Oi9ETAO6XRbqB8LDL3tF4bMk9x/59PljcGbP5v38BSzCjD1VTwuO6iWA8uzDVAjw2fMNfcT2/LyRlMOsynblo3envlivtgHnKkZj6HqRrG5ltgwy5NsCQ7WwwYPkldhLTF+wUYAnq28+QnU=";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var tokenGenerate = new JwtSecurityToken(
            claims: new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, "test"),
                new Claim(JwtRegisteredClaimNames.Aud, "test"),
                new Claim(JwtRegisteredClaimNames.Sub, "1234567890"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            },

            expires: new DateTimeOffset(DateTime.Now.AddMinutes(1)).DateTime,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerate);
        var mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(mockSection => mockSection.Value).Returns(key);

        // Create the simulated service
        var tokenManagerService = ServiceUtilities.CreateTokenManagerService(
            out _, out _, out _,
            out Mock<IConfiguration> configuration
            );

        // Configurations for tests
        configuration.Setup(configuration => configuration["Authentication:Jwt:SecretKey"]
        ).Throws(new Exception("Error to GetPrincipalFromToken"));

        var result = tokenManagerService.GetPrincipalFromExpiredToken(token);

        // Validations for tests
        Assert.False(result.IsSuccess);
    }
}

