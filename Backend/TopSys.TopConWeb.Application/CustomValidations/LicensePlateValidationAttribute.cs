using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class LicensePlateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success; // Leave presence or absence validation to RequiredAttribute
        }

        var licensePlate = value.ToString().Replace("-", "").Trim();

        // Check if the character at position 4 is a letter
        if (licensePlate.Length > 4 && char.IsLetter(licensePlate, 4))
        {
            // Check if the license plate is in the format: three letters, one number, one letter, and two numbers (Mercosul pattern)
            var mercosulPattern = new Regex("[a-zA-Z]{3}[0-9]{1}[a-zA-Z]{1}[0-9]{2}");
            if (mercosulPattern.IsMatch(licensePlate))
            {
                return ValidationResult.Success;
            }
        }
        else
        {
            // Check if the first 3 characters are letters and the last 4 characters are numbers
            var normalPattern = new Regex("[a-zA-Z]{3}[0-9]{4}");
            if (normalPattern.IsMatch(licensePlate))
            {
                return ValidationResult.Success;
            }
        }

        // If it doesn't match any of the patterns, return an error message
        var errorMessage = ErrorMessageString ?? "Invalid license plate format.";
        return new ValidationResult(errorMessage);
    }
}
