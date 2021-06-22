using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.TestData
{
    public static class DataFactory
    {
        public static Person CreatePerson() =>
        new Person
        {
            Address = new Address
            {
                City = "Awesome Town",
                State = "TN",
                Zip = new ZipCode(),
            },
            Name = "Josh",
            Children = new Person[]
                {
                    new Person()
                    {
                        Name = "Child 1",
                                        Address = new Address()
                                        {
                                            City = "City 1"
                                        }
                     }

                }
        };
    }
}
 