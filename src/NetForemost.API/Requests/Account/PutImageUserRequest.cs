using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account
{
    public class PutImageUserRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserImageUrl { get; set; }
    }
}
