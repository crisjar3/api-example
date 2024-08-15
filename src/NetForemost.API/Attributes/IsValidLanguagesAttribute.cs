using NetForemost.SharedKernel.Properties;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NetForemost.API.Attributes;
public class IsValidLanguagesAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            string pattern = @"^\d+,\d+(?::\d+,\d+)*$";

            string languages = value.ToString();

            if (!Regex.IsMatch(languages, pattern))
            {
                return new ValidationResult(ErrorStrings.FormatLanguagesIsNotValid);
            }
        }
        return ValidationResult.Success;
    }
}

