using System.ComponentModel.DataAnnotations;

namespace NetForemost.Report.API.Attributes
{
    public class DateGreaterThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateValue;
            if (DateTime.TryParse(value.ToString(), out dateValue))
            {
                if (dateValue > DateTime.Today)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("DateTime has to be greater than today.");
                }
            }

            else
                return new ValidationResult("Unable to parse date provided");
        }

    }
}
