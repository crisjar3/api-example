//using NetForemost.Report.API.Requests.Timers;
//using System;
//using System.ComponentModel.DataAnnotations;

//namespace NetForemost.Report.API.Attributes;

//public class DateRangeValidationAttribute : ValidationAttribute
//{
//    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//    {
//        if (value is not null)
//        {
//            var request = (PostDailyTimeBlockRequest)validationContext.ObjectInstance;
//            DateTime StartTime = request.TimeStart;
//            DateTime endTime = request.TimeEnd;

//            if (endTime <= StartTime)
//            {
//                return new ValidationResult("The date 'EndTime' must be greather than 'StartTime'.");
//            }
//        }


//        return ValidationResult.Success;
//    }
//}
