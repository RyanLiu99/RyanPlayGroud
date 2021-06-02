using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleApp1.Validation.TestData
{

    public class Person
    {
        [Required]
        public String Name { get; set; }

        [Required, ValidateNested]
        public Address Address { get; set; }
    }

    public class Address
    {
        [Required]
        public String Street1 { get; set; }

        public String Street2 { get; set; }

        [Required]
        public String City { get; set; }

        [Required]
        public String State { get; set; }

        [Required, ValidateNested]
        public ZipCode Zip { get; set; }
    }

    public class ZipCode
    {
        [Required]
        public String PrimaryCode { get; set; }

        public String SubCode { get; set; }
    }
}
