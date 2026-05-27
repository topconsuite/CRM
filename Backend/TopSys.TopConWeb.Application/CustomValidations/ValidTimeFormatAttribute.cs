using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class ValidTimeFormatAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            if (value is string timeString)
            {
                if (DateTime.TryParseExact(timeString, "HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    return ValidationResult.Success;
                }
            }

            var errorMessage = ErrorMessageString ?? $"The time format should be 'HH:mm'.";
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}