using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PutUserRequest
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    public int CityId { get; set; }
    public int TimeZoneId { get; set; }
    public int SeniorityId { get; set; }
}