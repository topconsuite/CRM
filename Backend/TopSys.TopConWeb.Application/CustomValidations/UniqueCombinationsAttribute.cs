using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TopSys.TopConWeb.Application.CustomValidations
{
    public class UniqueCombinationsAttribute : ValidationAttribute
    {
        private readonly string[] FieldNames;

        public UniqueCombinationsAttribute(params string[] fieldNames)
        {
            this.FieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IEnumerable<object>;
            var index = 0;

            if (list != null)
            {
                var uniqueCombinations = new HashSet<string>();

                foreach (var item in list)
                {
                    var combination = string.Join("|", FieldNames.Select(fieldName => item.GetType().GetProperty(fieldName)?.GetValue(item)?.ToString()));

                    if (!uniqueCombinations.Add(combination))
                    {
                        var errorMessage = ErrorMessageString ?? "Duplicate objects in the list.";
                        return new ValidationResult(errorMessage, new[] { $"{validationContext.MemberName}[{index}].combination" });
                    }
                    index++;
                }
            }

            return ValidationResult.Success;
        }
    }
}