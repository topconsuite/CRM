using System;
using System.ComponentModel.DataAnnotations;

namespace TopSys.TopConWeb.Application.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfEqualAttribute : ValidationAttribute
    {
        private string OtherPropertyName { get; }
        private object TargetValue { get; }

        public RequiredIfEqualAttribute(string otherPropertyName, object targetValue)
        {
            OtherPropertyName = otherPropertyName;
            TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            if (otherPropertyInfo == null)
                throw new ArgumentException($"Property with name {OtherPropertyName} not found");

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (Equals(otherPropertyValue, TargetValue) && (string.IsNullOrWhiteSpace(value?.ToString()) || value?.ToString() == "0"))
            {
                return new ValidationResult(ErrorMessageString ?? $"{validationContext.DisplayName} is required when {OtherPropertyName} is equal to {TargetValue}.");
            }

            return ValidationResult.Success;
        }
    }
}
