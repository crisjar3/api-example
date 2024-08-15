namespace NetForemost.Core.Dtos.Authorizations;

public class AuthorizedUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string AccessToken { get; set; }
    public int AuthTokenValidityInMins { get; set; }
    public string RefreshToken { get; set; }
    public int RefreshTokenValidityInDays { get; set; }
    public string? UserImageUrl { get; set; }
}