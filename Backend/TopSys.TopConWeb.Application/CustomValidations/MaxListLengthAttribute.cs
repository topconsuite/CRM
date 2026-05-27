using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TopSys.TopConWeb.Application.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxListLengthAttribute : ValidationAttribute
    {
        private readonly int maxLength;

        public MaxListLengthAttribute(int maxLength)
        {
            if (maxLength <= 0)
            {
                throw new ArgumentException("Maximum length must be greater than zero.", nameof(maxLength));
            }

            this.maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable list && list.Cast<object>().Count() > maxLength)
            {
                var errorMessage = ErrorMessageString ??  $"The list cannot have more than {maxLength} elements.";
                return new ValidationResult(errorMessage, new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
