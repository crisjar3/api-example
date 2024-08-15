using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetForemost.API.Attributes;

namespace NetForemost.API.Requests.Projects
{
    public class PostProjectAvatarRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        [Required]
        public int ProjectId { get; set; }
        [Required]
        [ValidTypeFile(fileType: FileType.Image)]
        [CustomFileSize(20)]
        public List<IFormFile> Images { get; set; }
    }
}
