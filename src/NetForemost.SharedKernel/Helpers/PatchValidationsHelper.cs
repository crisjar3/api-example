using Ardalis.Result;
using Microsoft.AspNetCore.JsonPatch;
using NetForemost.SharedKernel.Properties;

namespace NetForemost.SharedKernel.Helpers;

public static class PatchValidationHelper
{
    public static Result ValidatePatchDocument<T>(JsonPatchDocument<T> patchCompany) where T: class
    {
        List<string> invalidProperties = new();

        foreach (var operation in patchCompany.Operations)
        {
            if (operation.value == null || (operation.value is string stringValue && string.IsNullOrWhiteSpace(stringValue)))
            {
                // Get the property name from the "path" property (assuming the path is like "/propertyName")
                string propertyName = operation.path.TrimStart('/');

                invalidProperties.Add(propertyName);
            }
        }

        if (invalidProperties.Any())
        {
            // Construct the error message with the list of invalid properties
            string errorMessage = ErrorStrings.PatchEmptyPropsError.Replace(
                "[emptyProps]",
                string.Join(", ", invalidProperties)
            );
            return Result.Error(errorMessage);
        }

        return Result.Success();
    }
}