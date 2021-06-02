using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleApp1.Validation
{
    /// <summary>
    ///     Validates nested attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public  class ValidateEnumerableAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Overriden. Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">
        ///     The value to validate.
        /// </param>
        /// <param name="validationContext">
        ///     The context information about the validation operation.
        /// </param>
        /// <returns>
        ///     An instance of the <see cref="System.ComponentModel.DataAnnotations.ValidationResult"/> class.
        ///     Never <see langword="null"/>.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;

            if (value != null)
            {
                foreach (var nestedObject in GetNestedObjects(value))
                {
                    try
                    {
                        System.ComponentModel.DataAnnotations.Validator.ValidateObject(nestedObject, new ValidationContext(nestedObject), true);
                    }
                    catch (ValidationException x)
                    {
                        result = x.ValidationResult;
                        break;
                    }
                }
            }

            return result;
        }

        private IEnumerable GetNestedObjects(object value)
        {
            yield return value;


            if (value is IEnumerable enumerable)
            {
                foreach (var item in (IEnumerable)value)
                {
                    if (item != null)
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
