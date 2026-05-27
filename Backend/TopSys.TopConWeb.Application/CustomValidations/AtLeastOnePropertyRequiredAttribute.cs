using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TopSys.TopConWeb.Application.CustomValidations
{
    public class AtLeastOnePropertyRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value);
                if (propertyValue != null && !IsDefaultValue(property.PropertyType, propertyValue))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("At least one property must be provided.");
        }

        private bool IsDefaultValue(Type type, object value)
        {
            return value.Equals(type.IsValueType ? Activator.CreateInstance(type) : null);
        }
    }

}
