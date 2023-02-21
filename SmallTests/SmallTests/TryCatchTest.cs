using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SmallTests.Helpers;

namespace SmallTests
{
    internal class TryCatchTest
    {
        /*  Result:

            info: TestHelpers[0]
      	            Inner Try block
            info: TestHelpers[0]
      	            Inner Caught exception, before rethrow
            info: TestHelpers[0]
      	            Inner Finally
            info: TestHelpers[0]
                  Outer Caught ex again
            info: TestHelpers[0]
                  Outer Done testing         
         */
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