namespace NetForemost.API.Requests.Account;

public class PostResetPasswordRequest
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
}