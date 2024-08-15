using Microsoft.AspNetCore.JsonPatch;
using Moq;
using Xunit;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Specifications.Companies;
using NetForemost.UnitTests.Common;
using NetForemost.SharedKernel.Properties;

public class PatchCompanySettingsAsyncTests
{
    // Patch an existing CompanySettings with valid data
    [Fact]
    public async Task WhenPatchingExistingCompanySettingsSettingsWithValidData_ReturnSuccess()
    {
        // Given
        var companySettingsService = ServiceUtilities.CreateCompanySettingsService(
            out _,
            out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
        );

        var userId = "user1";
        var patchCompanySettings = new JsonPatchDocument<CompanySettings>();
        var newUpdatedByValue = "New UpdatedBy Value";
        patchCompanySettings.Replace(c => c.UpdatedBy, newUpdatedByValue);

        var existingCompanySettings = new CompanySettings
        {
            Id = 1,
            UpdatedBy = "Jhon Doe",
            AllowManagersCreateProjects = true,
            CompanyId = 1,
            UsePlayroll = true
        };

        companySettingsRepository.Setup(repository =>
            repository.FirstOrDefaultAsync(
                It.IsAny<GetCompanySettingsByUserIdSpecification>(),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(existingCompanySettings);

        // When
        var result = await companySettingsService.PatchCompanySettingsAsync(patchCompanySettings, userId);

        // Then
        Assert.True(result.IsSuccess);
        Assert.Equal(newUpdatedByValue, result.Value.UpdatedBy);
    }


    // Patch an existing CompanySettings with the same data
    [Fact]
    public async Task WhenPatchingExistingCompanySettingsWithSameData_ReturnSuccess()
    {
        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanySettingsService(
            out _,
            out Mock<IAsyncRepository<CompanySettings>> companySettingsRepository
            );

        var userId = "user1";
        var patchCompany = new JsonPatchDocument<CompanySettings>();
        var companySettings = new CompanySettings { Id = 1, CreatedBy = "Test Name", UpdatedBy = "Test Name" };

        companySettingsRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanySettingsByUserIdSpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(companySettings);
        companySettingsRepository.Setup(companyRepository => companyRepository.UpdateAsync(
                It.IsAny<CompanySettings>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        // When
        var result = await companyService.PatchCompanySettingsAsync(patchCompany, userId);

        // Then
        Assert.True(result.IsSuccess);
        Assert.Equal(companySettings, result.Value);
    }

    // Patch an existing CompanySettings with invalid data (e.g. empty name, invalid city id, invalid time zone id)
    [Fact]
    public async Task WhenPatchExistingCompanySettingsWithInvalidData_ReturnErrorWithInvalidProperties()
    {
        //Declarate Simulated Service
        var CompanySettingsService = ServiceUtilities.CreateCompanySettingsService(
            out _,
            out Mock<IAsyncRepository<CompanySettings>> CompanySettingsRepository
            );

        var userId = "user1";
        var patchCompanySettings = new JsonPatchDocument<CompanySettings>();
        patchCompanySettings.Replace(c => c.Id, 0);
        patchCompanySettings.Replace(c => c.CreatedBy, "");
        patchCompanySettings.Replace(c => c.UpdatedBy, "");

        var existingCompanySettings = new CompanySettings { Id = 1, CreatedBy = "Jhon Doe", UpdatedBy = "Jhon Doe" };
        CompanySettingsRepository.Setup(CompanySettingsRepository => CompanySettingsRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanySettingsByUserIdSpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(existingCompanySettings);

        // When
        var result = await CompanySettingsService.PatchCompanySettingsAsync(patchCompanySettings, userId);

        // Then
        Assert.False(result.IsSuccess);
        Assert.Equal(
            ErrorStrings.PatchEmptyPropsError.Replace(
                    "[emptyProps]",
                    string.Join(", ", new List<string> { "CreatedBy", "UpdatedBy" })
                ),
            result.Errors.First()
            );
    }

    // Try to patch an existing CompanySettings with an invalid patch document (e.g. patching a required field to null), and verify that the CompanySettings is not patched and the appropriate error message is returned.
    [Fact]
    public async Task WhenPatchingExistingCompanySettingsWithInvalidPatchDocument_ReturnAPatchError()
    {
        //Declarate Simulated Service
        var CompanySettingsService = ServiceUtilities.CreateCompanySettingsService(
            out _,
            out Mock<IAsyncRepository<CompanySettings>> CompanySettingsRepository
            );

        var userId = "user1";
        var patchCompanySettings = new JsonPatchDocument<CompanySettings>();
        patchCompanySettings.Replace(c => c.CreatedBy, null); // Patching a required field to null

        var existingCompanySettings = new CompanySettings
        {
            Id = 1,
            AllowManagersCreateProjects = true,
            CreatedBy = "Jhon Doe"
        };

        CompanySettingsRepository.Setup(CompanySettingsRepository => CompanySettingsRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanySettingsByUserIdSpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(existingCompanySettings);

        // Act
        var result = await CompanySettingsService.PatchCompanySettingsAsync(patchCompanySettings, userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(
            ErrorStrings.PatchEmptyPropsError.Replace(
                    "[emptyProps]",
                    string.Join(", ", new List<string> { "CreatedBy" })
                ),
            result.Errors.First()
            );
    }
}
