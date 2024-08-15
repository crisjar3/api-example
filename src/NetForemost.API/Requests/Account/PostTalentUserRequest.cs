using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PostTalentUserRequest
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
    [Range(0, int.MaxValue)]
    public int CityId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int SeniorityId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int JobRoleId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int TimeZoneId { get; set; }

    [Required]
    public List<PostUserSkillRequest> UserSkills { get; set; }

    [Required]
    public List<PostUserLanguageRequest> UserLanguages { get; set; }
}