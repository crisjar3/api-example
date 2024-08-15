using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Authentication;

public class PostLoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}