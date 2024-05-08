using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTests
{
    [TestClass]
    public class StaticPropertyAreNotSameEachAccessTest
    {
        [TestMethod]
        public void TestStaticProperties()
        {
            var success1 = DoActionResult.Success;
            var success2 = DoActionResult.Success;

            //Static property is static class,  each 
            Assert.IsFalse(success1 == success2);
            Assert.IsFalse(object.ReferenceEquals(success1, success2));

        }
    }



    public class DoActionResult
    {

        public bool IsSuccessful { get; protected set; }
        public string FailMessage { get; protected set; }



        public static DoActionResult Success
        {

            get
            {
                Console.WriteLine("static property called");
                return new DoActionResult
                {
                    IsSuccessful = true,
                    FailMessage = string.Empty,

                };
            }
        }

    }
}
