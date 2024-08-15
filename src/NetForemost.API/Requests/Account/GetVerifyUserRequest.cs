using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account
{
    public class GetVerifyUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

    }
}
