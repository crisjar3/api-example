using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Countries;
using NetForemost.Core.Entities.Industries;
using NetForemost.Core.Specifications.Companies;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

public class PatchCompanyDetailsAsyncTest
{
    // Patch an existing company with valid data
    [Fact]
    public async Task WhenPatchingExistingCompanyWithValidData_ReturnSuccess()
    {
        // Given
        var companyService = ServiceUtilities.CreateCompanyService(
                out _,
                out Mock<IAsyncRepository<Company>> companyRepository,
                out _,
                out _,
                out _,
                out _
                );

        var userId = "user1";
        var patchCompany = new JsonPatchDocument<Company>();
        patchCompany.Replace(c => c.Name, "Patched Company");

        var existingCompany = new Company { Id = 1, Name = "Test Company", CityId = 1, TimeZoneId = 1, IndustryId = 1 };

        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyByCompanyIdSpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(existingCompany);

        // When
        var result = await companyService.PatchCompanyDetailsAsync(userId, patchCompany, 1);

        // Then
        Assert.True(result.IsSuccess);
        Assert.Equal(patchCompany.Operations[0].value.ToString(), result.Value.Name);
    }

    // Patch an existing company with the same data
    [Fact]
    public async Task WhenPatchingExistingCompanyWithSameData_ReturnSuccess()
    {
        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<Industry>> industryRepository
            );

        var userId = "user1";
        var patchCompany = new JsonPatchDocument<Company>();
        
        var company = new Company { Id = 1, Name = "Test Company", CityId = 1, TimeZoneId = 1, IndustryId = 1 };

        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyByCompanyIdSpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(company);
        companyRepository.Setup(companyRepository => companyRepository.UpdateAsync(
                It.IsAny<Company>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        // When
        var result = await companyService.PatchCompanyDetailsAsync(userId, patchCompany, 1);

        // Then
        Assert.True(result.IsSuccess);
        Assert.Equal(company, result.Value);
    }

    // Patch an existing company with invalid data (e.g. empty name, invalid city id, invalid time zone id)
    [Fact]
    public async Task WhenPatchExistingCompanyWithInvalidData_ReturnErrorWithInvalidProperties()
    {
        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out Mock<IAsyncRepository<City>> cityRepository,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out Mock<IAsyncRepository<NetForemost.Core.Entities.TimeZones.TimeZone>> timeZoneRepository,
            out _,
            out _,
            out Mock<IAsyncRepository<Industry>> industryRepository
            );

        var userId = "user1";
        var patchCompany = new JsonPatchDocument<Company>();
        patchCompany.Replace(c => c.Name, "");
        patchCompany.Replace(c => c.Address2, "");
        patchCompany.Replace(c => c.CityId, 0);
        patchCompany.Replace(c => c.TimeZoneId, 0);

        var existingCompany = new Company { Id = 1, Name = "Test Company", CityId = 1, TimeZoneId = 1, IndustryId = 1 };
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyByCompanyIdSpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(existingCompany);

        cityRepository.Setup(cityRepository => cityRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new City());

        timeZoneRepository.Setup(timeZoneRepository => timeZoneRepository.GetByIdAsync(
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NetForemost.Core.Entities.TimeZones.TimeZone());

        // When
        var result = await companyService.PatchCompanyDetailsAsync(userId, patchCompany, 1);

        // Then
        Assert.False(result.IsSuccess);
        Assert.Equal(
            ErrorStrings.PatchEmptyPropsError.Replace(
                    "[emptyProps]",
                    string.Join(", ", new List<string> { "Name", "Address2" })
                ),
            result.Errors.First()
            );
    }

    // Try to patch an existing company with an invalid patch document (e.g. patching a required field to null), and verify that the company is not patched and the appropriate error message is returned.
    [Fact]
    public async Task WhenPatchingExistingCompanyWithInvalidPatchDocument_ReturnAPatchCompanyError()
    {
        //Declarate Simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out _,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _
            );

        var userId = "user1";
        var patchCompany = new JsonPatchDocument<Company>();
        patchCompany.Replace(c => c.Name, null); // Patching a required field to null

        var existingCompany = new Company
        {
            Id = 1,
            Name = "Existing Company",
            Address1 = "Test 1",
            Address2 = "Test 2"
        };

        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
                It.IsAny<GetCompanyByCompanyIdSpecification>(),
                It.IsAny<CancellationToken>())).
                ReturnsAsync(existingCompany);

        // Act
        var result = await companyService.PatchCompanyDetailsAsync(userId, patchCompany, 1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(
            ErrorStrings.PatchEmptyPropsError.Replace(
                    "[emptyProps]",
                    string.Join(", ", new List<string> { "Name" })
                ),
            result.Errors.First()
            );
    }
}
