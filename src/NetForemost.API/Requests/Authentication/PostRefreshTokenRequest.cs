using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Authentication;

public class PostRefreshTokenRequest
{
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}