using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PostConfirmEmailTokenRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}