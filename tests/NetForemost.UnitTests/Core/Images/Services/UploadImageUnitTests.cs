using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Moq;
using NetForemost.Core.Entities.CDN;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using NetForemost.UnitTests.Common;
using Xunit;

namespace NetForemost.UnitTests.Core.Images.Services;

public class UploadImageUnitTests
{
    /// <summary>
    /// Verify the correct functioning of the entire process to Upload an Image File
    /// </summary>
    /// <returns> Return Success if all is correct </returns>
    [Fact]
    public async Task WhenUploadImageIsCorrect_ReturnSuccess()
    {
        // Declarations of variables
        var files = new List<IFormFile>();
        var urlsFiles = new Google.Apis.Storage.v1.Data.Object()
        {
            Bucket = "bucket",
            Name = "Name",
            ContentType = "Image",
        };

        var file = new FormFile(
            baseStream: new MemoryStream(10),
            baseStreamOffset: 0,
            length: 10,
            name: "Image",
            fileName: "image.png"
        )
        {
            Headers = new HeaderDictionary(),
            ContentType = "Content;Content"
        };

        files.Add(file);

        // Create the simulated service
        var imageStorageService = ServiceUtilities.CreateImageStorageService(
            out Mock<StorageClient> storageClient,
            out Mock<IOptions<CloudStorageConfig>> config
            );

        // Configurations for tests
        storageClient.Setup(storageClient => storageClient.UploadObjectAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<MemoryStream>(), null,
            It.IsAny<CancellationToken>(), null
            )).ReturnsAsync(urlsFiles);

        config.Setup(config => config.Value).Returns(new CloudStorageConfig());

        var result = await imageStorageService.UploadImage(files);

        // Validations for tests
        Assert.True(result.IsSuccess);
    }

    /// <summary>
    /// It checks if the Image List is null or empty and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return ImageFileEmpty</returns>
    [Fact]
    public async Task WhenImageListIsNullOrEmpty_ReturnImageFileEmpty()
    {
        // Declarations of variables
        var files = new List<IFormFile>();
        var urlsFiles = new Google.Apis.Storage.v1.Data.Object()
        {
            Bucket = "bucket",
            Name = "Name",
            ContentType = "Image",
        };

        // Create the simulated service
        var imageStorageService = ServiceUtilities.CreateImageStorageService(
            out Mock<StorageClient> storageClient,
            out Mock<IOptions<CloudStorageConfig>> config
            );

        // Configurations for tests
        storageClient.Setup(storageClient => storageClient.UploadObjectAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<MemoryStream>(), null,
            It.IsAny<CancellationToken>(), null
            )).ReturnsAsync(urlsFiles);

        config.Setup(config => config.Value).Returns(new CloudStorageConfig());

        var result = await imageStorageService.UploadImage(files);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ImageFileEmpty);
    }

    /// <summary>
    /// It checks if an Image file is null or empty and does'nt finish the process correctly.
    /// </summary>
    /// <returns> Return ImageFileEmpty</returns>
    [Fact]
    public async Task WhenOneOrMoreImagesAreEmpty_ReturnImageFileEmpty()
    {
        // Declarations of variables
        var files = new List<IFormFile>();
        var urlsFiles = new Google.Apis.Storage.v1.Data.Object()
        {
            Bucket = "bucket",
            Name = "Name",
            ContentType = "Image",
        };

        files.Add(null);

        // Create the simulated service
        var imageStorageService = ServiceUtilities.CreateImageStorageService(
            out Mock<StorageClient> storageClient,
            out Mock<IOptions<CloudStorageConfig>> config
            );

        // Configurations for tests
        storageClient.Setup(storageClient => storageClient.UploadObjectAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<MemoryStream>(), null,
            It.IsAny<CancellationToken>(), null
            )).ReturnsAsync(urlsFiles);

        config.Setup(config => config.Value).Returns(new CloudStorageConfig());

        var result = await imageStorageService.UploadImage(files);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorHelper.GetValidationErrors(result.ValidationErrors), ErrorStrings.ImageFileEmpty);
    }

    /// <summary>
    /// It checks if an unexpected error occurs, is detected and does not interrupt the process.
    /// </summary>
    /// <returns> Return Error </returns>
    [Fact]
    public async Task WhenAnUnexpectedErrorsOccurToUploadImages_ReturnError()
    {
        // Declarations of variables
        var testError = "An Error occurs while upload one or more images";
        var files = new List<IFormFile>();
        var urlsFiles = new Google.Apis.Storage.v1.Data.Object()
        {
            Bucket = "bucket",
            Name = "Name",
            ContentType = "Image",
        };

        var file = new FormFile(
            baseStream: new MemoryStream(10),
            baseStreamOffset: 0,
            length: 10,
            name: "Image",
            fileName: "image.png"
        )
        {
            Headers = new HeaderDictionary(),
            ContentType = "Content;Content"
        };

        files.Add(file);

        // Create the simulated service
        var imageStorageService = ServiceUtilities.CreateImageStorageService(
            out Mock<StorageClient> storageClient,
            out Mock<IOptions<CloudStorageConfig>> config
            );

        // Configurations for tests
        storageClient.Setup(storageClient => storageClient.UploadObjectAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<MemoryStream>(), null,
            It.IsAny<CancellationToken>(), null
            )).Throws(new Exception(testError));

        config.Setup(config => config.Value).Returns(new CloudStorageConfig());

        var result = await imageStorageService.UploadImage(files);

        // Validations for tests
        Assert.False(result.IsSuccess);
        Assert.Contains(ErrorHelper.GetErrors(result.Errors.ToList()), testError);
    }
}
