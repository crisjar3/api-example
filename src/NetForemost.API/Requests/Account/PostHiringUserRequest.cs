using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PostUserRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string UserName { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Phone]
    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public int CityId { get; set; }
}