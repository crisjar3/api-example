using Microsoft.AspNetCore.Http;
using NetForemost.API.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Images;

public class PostImageRequest
{
    [Required]
    [ValidTypeFile(fileType: FileType.Image)]
    [CustomFileSize(20)]
    public List<IFormFile> Images { get; set; }
}