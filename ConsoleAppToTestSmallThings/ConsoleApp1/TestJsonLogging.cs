using ConsoleApp1.TestData;
using ConsoleApp1.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class TestJsonLogging
    {
        public static void Test()
        {
            TestJsonLogging tl = Program.Container.GetService(typeof(TestJsonLogging)) as TestJsonLogging;
            var person = DataFactory.CreatePerson();
            tl.LogSth(person);
        }

        private readonly ILogger<TestJsonLogging> _logger;

        public TestJsonLogging(ILogger<TestJsonLogging> logger)
        {
            this._logger = logger;
        }

        public void LogSth(Object sth)
        {
            //Ryan test:
            //1: When writng to console, it is not real time. There is delay.
            //2: ToDebugString is needed, otherwise just class name

            _logger.LogInformation("Logging sth: {@sth}", sth); //.ToDebugString()
        }
    }
}
