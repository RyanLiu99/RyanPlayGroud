using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ConsoleApp1
{
    class Program
    {
        internal static ServiceProvider Container;

        static void Main(string[] args)
        {
            OneTimeSetup();
            //TestDateTime.Test(); //long delay, for testing date time over new year midnight
            TestValidation.Test();
            //TestJsonLogging.Test();
        }

        private static void OneTimeSetup()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddDebug());
            services.AddLogging(configure => configure.AddSimpleConsole(c =>
            {
                c.ColorBehavior = LoggerColorBehavior.Enabled;
                c.UseUtcTimestamp = false;
                c.TimestampFormat = "[yy-MM-dd HH:mm:ss]";
            }));

            RegisterDependency(services);

            Container = services.BuildServiceProvider();
        }


        private static void RegisterDependency(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<TestJsonLogging>();
        }
    }
}



