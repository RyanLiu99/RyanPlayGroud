using ConsoleApp1.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConsoleApp1.TestData
{

    public class Person
    {
        [Required]
        public string Name { get; set; }

        [ValidateObject]
        public Address Address { get; set; }

        [ValidateEnumerable]
        public Person[] Children { get; set; }
    }

    public class Address
    {
        [Required]
        public string Street1 { get; set; }

        public string Street2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required, ValidateObject]
        public ZipCode Zip { get; set; }

    }

    public class ZipCode
    {
        [Required]
        public string PrimaryCode { get; set; }

        public string SubCode { get; set; }
    }
}
