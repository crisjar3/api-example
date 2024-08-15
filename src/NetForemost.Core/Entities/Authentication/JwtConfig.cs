namespace NetForemost.Core.Entities.Authentication;

public class JwtConfig
{
    public int AuthTokenValidityInMins { get; set; }
    public int RefreshTokenValidityInDays { get; set; }
}