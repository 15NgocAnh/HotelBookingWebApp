using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Data.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AgeRequirementAttribure : ValidationAttribute
    {
        private readonly int _minAge;

        public AgeRequirementAttribure(int minAge) 
        { 
            _minAge = minAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dateOfBirth = value as DateOnly?;
            var memberNames = new List<string>() { validationContext.MemberName };
            if (dateOfBirth != null)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - dateOfBirth.Value.Year;
                // Check to see if DOB + AGE is greater than the date now
                if (dateOfBirth.Value > today.AddYears(-age)) age--;
                if (age < _minAge)
                {
                    return new ValidationResult($"Age must be at least {_minAge} years old", memberNames);
                }
            }
            return ValidationResult.Success;
        }
    }
}
