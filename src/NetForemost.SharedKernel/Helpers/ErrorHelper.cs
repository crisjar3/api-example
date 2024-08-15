using Ardalis.Result;

namespace NetForemost.SharedKernel.Helpers
{
    public static class ErrorHelper
    {
        public static string GetValidationErrors(List<ValidationError> errors)
        {
            return string.Join(",", errors.Select(x => x.ErrorMessage));
        }

        public static string GetErrors(List<string> errors)
        {
            return string.Join(",", errors);
        }

        public static string GetExceptionError(Exception ex)
        {
            return ex.InnerException is not null ? ex.Message + " | " + ex.InnerException.Message : ex.Message;
        }

        public static List<ValidationError> Error(string message, List<int> ids = null)
        {
            return new List<ValidationError>() { new ValidationError { ErrorMessage = message + (ids != null ? $": {string.Join(", ", ids)}" : "") } };
        }
    }
}
