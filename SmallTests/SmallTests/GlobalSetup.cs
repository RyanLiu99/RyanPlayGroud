using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;

namespace SmallTests
{
    [SetUpFixture]
    public static class GlobalSetup
    {
        internal static ServiceProvider? Container;

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            //services.AutoRegisterServices(configuration);

            services.AddLogging(configure => configure.AddDebug());
            services.AddLogging(configure => configure.AddSimpleConsole(c =>
            {
                c.ColorBehavior = LoggerColorBehavior.Enabled;
                c.UseUtcTimestamp = false;
                c.TimestampFormat = "[yy-MM-dd HH:mm:ss]";
            }));

            services.AddMemoryCache(options => options.TrackStatistics = true);
            Container = services.BuildServiceProvider();
            //Container.SetUpIocAdapter();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Container?.Dispose();
        }
    }
}

