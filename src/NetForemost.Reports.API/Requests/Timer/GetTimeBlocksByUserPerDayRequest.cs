using NetForemost.SharedKernel.Helpers;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.Reports.API.Requests.Timer;

public class GetTimeBlocksByUserPerDayRequest : DateRangeHelper
{
    [Required]
    public int CompanyUserId { get; set; }
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }
    [Required]
    public double TimeZone { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (From > DateTime.Now)
        {
            yield return new ValidationResult($"Invalid Date Range. The 'From date' {From.ToString("yyyy-MM-dd")} cannot be greater than the current date.");
        }

        if (From > To)
        {
            yield return new ValidationResult($"Invalid Date Range. The 'From date' {From.ToString("yyyy-MM-dd")} must be less than or equal to the 'To date {To.ToString("yyyy-MM-dd")}.");
        }
    }
}