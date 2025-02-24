using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    internal class ExceptionTest
    {
        [Test]
        public void TestException()
        {
            try
            {
                throw new Exception("Test exception");
            }
            catch (Exception ex)
            {
               Assert.AreEqual("System.Exception", ex.GetType().FullName);
            }
        }

        [Test]
        public void TestNullReferenceException()
        {
            try
            {
                throw new NullReferenceException("Null reference exception");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("System.NullReferenceException", ex.GetType().FullName);
            }
        }
    }
}
