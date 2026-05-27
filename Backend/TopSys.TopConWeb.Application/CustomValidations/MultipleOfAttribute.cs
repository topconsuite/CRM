using System;
using System.ComponentModel.DataAnnotations;

namespace TopSys.TopConWeb.Application.CustomValidations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MultipleOfAttribute : ValidationAttribute
    {
        private readonly int _divisor;

        public MultipleOfAttribute(int divisor)
        {
            if (divisor <= 0)
            {
                throw new ArgumentException("The divisor must be a positive integer greater than zero.");
            }

            _divisor = divisor;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            int intValue;
            if (int.TryParse(value.ToString(), out intValue) && intValue % _divisor == 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessageString ?? $"{validationContext.DisplayName} must be a multiple of {_divisor}.");
        }
    }
}
