using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateInTheFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = value as DateTime?;
            var memberNames = new List<string>() { validationContext.MemberName };
            if (date != null)
            {
                if (date.Value.Date < DateTime.UtcNow.Date)
                {
                    return new ValidationResult("This must be a date in the future", memberNames);
                }
            }
            return ValidationResult.Success;
        }
    }
}
