using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.API.Attributes;
public class CustomFileSize : ValidationAttribute
{
    private readonly int MaxFileSizeInBytes;

    public CustomFileSize(int maxSize)
    {
        MaxFileSizeInBytes = maxSize * 1024 * 1024; // MB
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        if (file != null)
        {
            if (file.Length > MaxFileSizeInBytes)
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}