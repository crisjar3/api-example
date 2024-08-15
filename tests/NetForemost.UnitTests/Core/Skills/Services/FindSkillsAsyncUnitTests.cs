using Moq;
using NetForemost.Core.Entities.Skills;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Skills.Services;

public class FindSkillsAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to find all Skill
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindAllSkillsAsyncIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var skills = new List<Skill>();

        // Create the simulated service
        var skillService = ServiceUtilities.CreateSkillService(out Mock<IAsyncRepository<Skill>> skillRepository);

        // Configurations for tests
        skillRepository.Setup(skillRepository => skillRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(skills);

        var result = await skillService.FindSkillsAsync();

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToFindAllSkills_ReturnError()
    {
        // Declarations of variables
        var skills = new List<Skill>();
        var testError = "Error to find all Skills";

        // Create the simulated service
        var skillService = ServiceUtilities.CreateSkillService(out Mock<IAsyncRepository<Skill>> skillRepository);

        // Configurations for tests
        skillRepository.Setup(skillRepository => skillRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await skillService.FindSkillsAsync();

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
