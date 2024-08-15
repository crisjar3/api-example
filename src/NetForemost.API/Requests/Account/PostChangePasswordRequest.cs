using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Requests.Account;

public class PostChangePasswordRequest
{
    [DataType(DataType.Password)]
    [Required]
    public string CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [Required]
    public string ConfirmNewPassword { get; set; }
}