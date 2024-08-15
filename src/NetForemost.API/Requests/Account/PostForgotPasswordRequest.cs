using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PostForgotPasswordRequest
{
    [EmailAddress] public string Email { get; set; }
}