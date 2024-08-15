using Microsoft.AspNetCore.Http;
using NetForemost.SharedKernel.Helpers;
using NetForemost.SharedKernel.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NetForemost.API.Attributes;
public class ValidTypeFile : ValidationAttribute
{
    private readonly string[] _validFileTypes;

    public ValidTypeFile(string[] validFileTypes)
    {
        _validFileTypes = validFileTypes;
    }

    public ValidTypeFile(FileType fileType)
    {
        if (fileType == FileType.Image)
        {
            _validFileTypes = new string[] { "image/jpg", "image/jpeg", "image/png" };
        }
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try
        {
            var formFiles = value as List<IFormFile>;

            // Check if image collection is null
            if (formFiles is null)
            {
                return new ValidationResult(ErrorStrings.ImageFileEmpty);
            }

            // Check that the format of each of the images in the list is valid
            foreach (var formFile in formFiles)
            {
                if (!_validFileTypes.Contains(formFile.ContentType))
                {
                    return new ValidationResult($"The file {formFile.FileName} must be of the following allowed file types {string.Join(",", _validFileTypes)}");
                }
            }

            return ValidationResult.Success;
        }
        catch (Exception ex)
        {
            return new ValidationResult(ErrorHelper.GetExceptionError(ex));
        }
    }
}

public enum FileType
{
    Image
}