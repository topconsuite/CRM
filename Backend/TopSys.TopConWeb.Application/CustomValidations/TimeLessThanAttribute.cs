using System;
using System.ComponentModel.DataAnnotations;

namespace TopSys.TopConWeb.Application.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TimeLessThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public TimeLessThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);

            if (otherPropertyInfo == null)
                throw new ArgumentException($"Property with name {_otherPropertyName} not found");

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (value == null || otherPropertyValue == null)
            {
                return ValidationResult.Success;
            }

            string timeA = value.ToString();
            string timeB = otherPropertyValue.ToString();

            if (IsTimeValid(timeA) && IsTimeValid(timeB))
            {
                var timeAValue = TimeSpan.Parse(timeA);
                var timeBValue = TimeSpan.Parse(timeB);

                if (timeAValue < timeBValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessageString ?? $"{validationContext.DisplayName} must be greater than {_otherPropertyName}.");
        }

        private bool IsTimeValid(string time)
        {
            return TimeSpan.TryParse(time, out _);
        }
    }
}
