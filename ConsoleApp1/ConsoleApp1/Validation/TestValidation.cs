using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ConsoleApp1.Validation.TestData;

namespace ConsoleApp1.Validation
{
    internal static class TestValidation
    {
        internal static void Test()
        {
            var person = new Person
            {
                Address = new Address
                {
                    City = "Awesome Town",
                    State = "TN",
                    Zip = new ZipCode()
                },
                Name = "Josh"
            };

            var context = new ValidationContext(person, null, null);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(person, context, results, true);

            PrintResults(results, 0);

            Console.ReadKey();
        }

        private static void PrintResults(IEnumerable<ValidationResult> results, Int32 indentationLevel)
        {
            foreach (var validationResult in results)
            {
                SetIndentation(indentationLevel);

                Console.WriteLine(validationResult.ErrorMessage);
                Console.WriteLine();

                if (validationResult is CompositeValidationResult)
                {
                    PrintResults(((CompositeValidationResult)validationResult).Results, indentationLevel + 1);
                }
            }
        }

        private static void SetIndentation(int indentationLevel)
        {
            Console.CursorLeft = indentationLevel * 4;
        }
    }
}
