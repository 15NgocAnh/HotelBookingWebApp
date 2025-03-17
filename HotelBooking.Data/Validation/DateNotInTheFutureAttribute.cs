using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateNotInTheFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = value as DateTime?;
            var memberNames = new List<string>() { validationContext.MemberName };
            if (date != null)
            {
                if (date.Value.Date > DateTime.UtcNow.Date)
                {
                    return new ValidationResult("It cannot be a future date", memberNames);
                }
            }
            return ValidationResult.Success;
        }
    }
}
