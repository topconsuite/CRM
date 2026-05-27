using System;
using System.ComponentModel.DataAnnotations;

namespace TopSys.TopConWeb.Application.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfNotEmptyAttribute : ValidationAttribute
    {
        private string OtherPropertyName { get; }

        public RequiredIfNotEmptyAttribute(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            if (otherPropertyInfo == null)
                throw new ArgumentException($"Property with name {OtherPropertyName} not found");

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (otherPropertyValue != null && !string.IsNullOrWhiteSpace(otherPropertyValue.ToString()) && (string.IsNullOrWhiteSpace(value?.ToString()) || value?.ToString() == "0"))
            {
                return new ValidationResult(ErrorMessageString ?? $"{validationContext.DisplayName} is required when {OtherPropertyName} is non-empty.");
            }

            return ValidationResult.Success;
        }
    }

}
