using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _endDatePropertyName;

        public DateRangeAttribute(string endDatePropertyName)
        {
            _endDatePropertyName = endDatePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            var endDateProperty = validationContext.ObjectType.GetProperty(_endDatePropertyName);
            var startDateValue = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);
            var endDateValue = (DateTime?)endDateProperty.GetValue(validationContext.ObjectInstance);
            if (startDateValue.HasValue && endDateValue.HasValue && startDateValue > endDateValue)
            {
                return new ValidationResult(ErrorMessage ?? "Start date must be less than end date");
            }
            return ValidationResult.Success;
        }
    }
}
