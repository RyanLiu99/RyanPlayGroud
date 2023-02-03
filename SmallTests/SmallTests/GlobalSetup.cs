using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using Microsoft.Extensions.Logging;

namespace SmallTests
{
    [SetUpFixture]
    public static class GlobalSetup
    {
        internal static IServiceProvider? Container;

        //[OneTimeSetUp]
        //public static void OneTimeSetup()
        //{
        //    IConfiguration configuration = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .Build();

        //    var services = new ServiceCollection();
        //    //services.AutoRegisterServices(configuration);

        //    services.AddLogging(configure => configure.AddDebug());
        //    services.AddLogging(configure => configure.AddSimpleConsole(c =>
        //    {
        //        c.ColorBehavior = LoggerColorBehavior.Enabled;
        //        c.UseUtcTimestamp = false;
        //        c.TimestampFormat = "[yy-MM-dd HH:mm:ss]";
        //    }));

        //    services.AddMemoryCache(options => options.TrackStatistics = true);
        //    Container = services.BuildServiceProvider();
        //    //Container.SetUpIocAdapter();
        //}


        [OneTimeSetUp]
        public static void OneTimeSetup() //cannot have parameter
        {
            OneTimeSetupInternal(null);
        }

        private static void OneTimeSetupInternal(Action<IServiceCollection, IConfiguration>? configureDelegate = null)
        {
            var host = CreateDefaultBuilder(configureDelegate).Build();
            Container = host.Services;
            //Container.SetUpIocAdapter();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            (Container as ServiceProvider)?.Dispose();
        }

        static IHostBuilder CreateDefaultBuilder(Action<IServiceCollection, IConfiguration>? configureDelegate = null)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services, context.Configuration);
                    configureDelegate?.Invoke(services, context.Configuration);
                });
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            
            //services.AutoRegisterServices(configuration);
            services.AddMemoryCache(options => options.TrackStatistics = true);
        }

    }
}

