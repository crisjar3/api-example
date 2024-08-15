using Moq;
using NetForemost.Core.Entities.Industries;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Interfaces;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Industries.Services;

public class FindIndustriesAsyncUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process of Find all Industries
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenFindIndustriesAsyncIsCorrect_ReturnSuccess()
    {
        //Declarations of variables
        var industries = new List<Industry>();

        //Create the simulated service
        var industryService = ServiceUtilities.CreateIndustryService(out Mock<IAsyncRepository<Industry>> industryRepository);

        //Configurations for tests
        industryRepository.Setup(industryRepository => industryRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(industries);

        var result = await industryService.FindIndustriesAsync();

        //Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// Verify the correct functioning of the entire process of order by Name all Industries
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenIndustriesOderByNameIsCorrect_ReturnSuccess()
    {
        //Declarations of variables
        var industries = new List<Industry>()
        {
            new Industry { Id = 1, Name = "D" },
            new Industry { Id = 2, Name = "A" },
            new Industry { Id = 3, Name = "C" },
            new Industry { Id = 4, Name = "B" },
        };

        var orderedIndustries = industries.OrderBy(industry => industry.Name).ToList();

        //Create the simulated service
        var industryService = ServiceUtilities.CreateIndustryService(out Mock<IAsyncRepository<Industry>> industryRepository);

        //Configurations for tests
        industryRepository.Setup(industryRepository => industryRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(industries);

        var result = await industryService.FindIndustriesAsync();
        var isEqual = ServiceUtilities.CompareList(result.Value.ToArray(), orderedIndustries.ToArray());

        //Validations for tests
        Assert.True(result.IsSuccess);
        Assert.True(isEqual);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorOccur_ReturnError()
    {
        //Declarations of variables
        var industries = new List<Industry>();
        var testError = "Error to find all Industries";

        //Create the simulated service
        var industryService = ServiceUtilities.CreateIndustryService(out Mock<IAsyncRepository<Industry>> industryRepository);

        //Configurations for tests
        industryRepository.Setup(industryRepository => industryRepository.ListAsync(
            It.IsAny<CancellationToken>()
            )).Throws(new Exception(testError));

        var result = await industryService.FindIndustriesAsync();

        //Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
