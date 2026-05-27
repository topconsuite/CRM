using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueFieldsAttribute : ValidationAttribute
{
    private readonly string[] fieldNames;

    public UniqueFieldsAttribute(params string[] fieldNames)
    {
        this.fieldNames = fieldNames ?? throw new ArgumentNullException(nameof(fieldNames));

        if (fieldNames.Length < 1)
        {
            throw new ArgumentException("Validation requires at least one field to compare.", nameof(fieldNames));
        }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var list = value as IEnumerable;

        if (list != null)
        {
            var fieldIndices = new Dictionary<string, HashSet<object>>();
            var index = 0;

            foreach (var fieldName in fieldNames)
            {
                fieldIndices[fieldName] = new HashSet<object>();
            }

            foreach (var item in list)
            {
                foreach (var fieldName in fieldNames)
                {
                    var fieldValue = GetFieldValue(item, fieldName);

                    if (!fieldIndices[fieldName].Add(fieldValue))
                    {
                        var errorMessage = ErrorMessageString ?? $"The {fieldName} field must be unique in the list.";

                        return new ValidationResult(ErrorMessageString, new[] { $"{validationContext.MemberName}[{index}].{fieldName}" });
                    }
                }

                index++;
            }
        }

        return ValidationResult.Success;
    }

    private object GetFieldValue(object item, string fieldName)
    {
        var propertyInfo = item.GetType().GetProperty(fieldName);

        if (propertyInfo != null)
        {
            return propertyInfo.GetValue(item);
        }

        throw new ArgumentException($"The field {fieldName} was not found in the class.", nameof(fieldName));
    }
}
