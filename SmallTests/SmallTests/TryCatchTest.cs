using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SmallTests.Helpers;

namespace SmallTests
{
    internal class TryCatchTest
    {
        [Test]
        public void TestTry()
        {
            try
            {
                Action();
            }
            catch (Exception e)
            {
                TestHelpers.Logger.Value.LogInformation("Outer Caught ex again");
            }

            TestHelpers.Logger.Value.LogInformation("Outer Done testing");
        }


        public void Action()
        {
            try
            {
                TestHelpers.Logger.Value.LogInformation("\tInner Try block");
                throw new Exception();
            }
            catch (Exception e)
            {
                TestHelpers.Logger.Value.LogInformation("\tInner Caught exception, before rethrow");
                throw;
            }
            finally
            {
                TestHelpers.Logger.Value.LogInformation("\tInner Finally");
            }

        }
    }

}