using Ardalis.Result;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NetForemost.Core.Entities.CDN;
using NetForemost.Core.Interfaces.Images;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.Core.Services.Images;

public class ImageStorageService : IImageStorageService
{
    private readonly StorageClient _storageClient;

    private readonly string _bucketName;

    private readonly string bucketDomainName = "https://storage.googleapis.com";

    public ImageStorageService(StorageClient storageClient, IOptions<CloudStorageConfig> config)
    {
        _storageClient = storageClient;
        _bucketName = config.Value.BucketName;
    }

    public async Task<Result<List<string>>> UploadImage(List<IFormFile> files)
    {
        try
        {
            // URL List
            var urls = new List<string>();

            // Verify if image list is null or empty
            if (files is null || files.Count == 0)
            {
                return Result.Invalid(
                    new()
                    {
                        new()
                        {
                            ErrorMessage = ErrorStrings.ImageFileEmpty
                        }
                    });
            }

            foreach (var file in files)
            {
                // Validate image file
                if (file is null || file.Length == 0)
                {
                    return Result.Invalid(
                        new()
                        {
                            new()
                            {
                                ErrorMessage = ErrorStrings.ImageFileEmpty
                            }
                        });
                }

                // Get FileName
                var fileName = Path.GetFileName(file.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var objectName = Guid.NewGuid().ToString();

                    await _storageClient.UploadObjectAsync(_bucketName, objectName, file.ContentType, memoryStream);

                    // Generate URLs public.
                    urls.Add($"{bucketDomainName}/{_bucketName}/{objectName}");
                }
            }
            return Result.Success(urls);
        }
        catch (Exception ex)
        {
            return Result.Error(ErrorHelper.GetExceptionError(ex));
        }
    }
}