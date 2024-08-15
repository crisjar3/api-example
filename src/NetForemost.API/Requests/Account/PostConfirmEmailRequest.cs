using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PostConfirmEmailRequest
{
    [Required] public string UserId { get; set; }

    [Required] public string Token { get; set; }
}