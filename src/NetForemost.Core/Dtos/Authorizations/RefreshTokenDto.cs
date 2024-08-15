namespace NetForemost.Core.Dtos.Authorizations;

public class RefreshTokenDto
{
    public string AccessToken { get; set; }
    public string TokenInfo { get; set; }
    public DateTime ValidTo { get; set; }
}