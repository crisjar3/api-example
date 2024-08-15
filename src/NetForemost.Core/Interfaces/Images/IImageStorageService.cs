using Ardalis.Result;
using Microsoft.AspNetCore.Http;

namespace NetForemost.Core.Interfaces.Images;

public interface IImageStorageService
{
    /// <summary>
    /// upload images for avatars
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    Task<Result<List<string>>> UploadImage(List<IFormFile> files);
}