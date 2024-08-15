using Moq;
using NetForemost.Core.Entities.Companies;
using NetForemost.Core.Entities.Users;
using NetForemost.Core.Specifications.Companies;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Companies.Services.CompanyService;

public class FindCompanyAsyncTest
{
    /// <summary>
    /// Verify the correct funtioning of the entire proccess
    /// </summary>
    /// <returns> IsSucess</returns>
    [Fact]
    public async Task WhenAllIsCorrect_ReturnIsSuccess()
    {
        //Declaration of Variables
        var companyId = 1;

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out _,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _
            );

        //Configuration For test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync(new Company() { Id = companyId });

        var result = await companyService.FindCompanyAsync(companyId);

        //Validation For Test
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Check when company is null
    /// </summary>
    /// <returns>Company not found</returns>
    [Fact]
    public async Task WhenCompanyNotFound_ErrorCompanyNotFound()
    {
        //Declaration of Variables
        var companyId = 1;

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out _,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _
            );

        //Configuration For test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            ReturnsAsync((Company)null);

        var result = await companyService.FindCompanyAsync(companyId);

        //Validation For Test
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetErrors(result.Errors.ToList()), ErrorStrings.CompanyNotFound);
    }

    /// <summary>
    /// Check when company is null
    /// </summary>
    /// <returns> Test Error</returns>
    [Fact]
    public async Task WhenUnexpectedError_ReturnUnexpectedError()
    {
        //Declaration of Variables
        var companyId = 1;
        var testError = "Test Error";

        //Create simulated Service
        var companyService = ServiceUtilities.CreateCompanyService(
            out _,
            out Mock<IAsyncRepository<Company>> companyRepository,
            out _,
            out _,
            out _,
            out _
            );

        //Configuration For test
        companyRepository.Setup(companyRepository => companyRepository.FirstOrDefaultAsync(
            It.IsAny<GetCompanyByCompanyIdSpecification>(),
            It.IsAny<CancellationToken>())).
            Throws(new Exception(testError));

        var result = await companyService.FindCompanyAsync(companyId);

        //Validation For Test
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}