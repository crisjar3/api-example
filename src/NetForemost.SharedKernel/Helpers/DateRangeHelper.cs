using NetForemost.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.SharedKernel.Helpers;

public class DateRangeHelper : IValidatableObject
{
    [Required]
    public DateTime From { get; set; }

    [Required]
    public DateTime To { get; set; }

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

public class DateRangeWithPaginationHelper : PaginationRequest, IValidatableObject
{
    [Required]
    public DateTime From { get; set; }

    [Required]
    public DateTime To { get; set; }

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