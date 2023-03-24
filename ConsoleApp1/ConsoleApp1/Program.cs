﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;

namespace ConsoleApp1
{
    class Program
    {
        
        internal static ServiceProvider Container;

        static void Main(string[] args)
        {
            //TestLogFactory();
            //TestLogFactory2();

            //OneTimeSetup();
            //TestDateTime.Test(); //long delay, for testing date time over new year midnight
            //TestValidation.Test();
            // TestJsonLogging.Test();

            //TestRegex.RunTests();

            //TestDynamic();
            //TestTupleValueTuple();

           new TestAsyncLocal().Test();
        }

        private static void TestTupleValueTuple()
        {
            Console.WriteLine( "ValueTuple equals Tuple:" + ("name", 18).Equals(Tuple.Create("name", 18)) ); //ValueTuple equals Tuple:False

            Dictionary<object, string> dic = new Dictionary<object, string>()
            {
                { ("name", 1), "ValueTuple 123" },
                { Tuple.Create("name", 1), "Tuple 999" }
            };

            Console.WriteLine("valueTuple is :" + dic[("name", 1)]);  //ValueTuple 123
            Console.WriteLine("Tuple is :" + dic[Tuple.Create("name", 1)]); //Tuple 999



            Dictionary<object, string> dic2 = new Dictionary<object, string>()
            {
                { Tuple.Create("name", 1), "Tuple 222" }
            };

            var found = dic2.TryGetValue(("name", 1), out string tt);
            Console.WriteLine($"valueTuple found {found}, is : {tt}");  //  valueTuple found False, is :
            Console.WriteLine("Tuple is :" + dic2[Tuple.Create("name", 1)]); //Tuple is :Tuple 222
        }

        private static void TestDynamic()
        {
            dynamic s = "============";
            dynamic d = "abcd222"; //will be gone, GC away
            d = 2;

            int i = 3;
            dynamic ii = 33;   //it will also boxed to object, same as below. dynamic is compile time concept, 
            object iii = 333;
            
            Debugger.Break();
        }

        private static void OneTimeSetup()
        {
            Debug.Assert(false, "debug.Assert failed");
            Serilog.Log.Logger = new LoggerConfiguration()
               // .Enrich.FromLogContext() //don't see difference
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] [{SourceContext}] {Message:lj} {NewLine}{Exception}")
               .WriteTo.File("log.txt",
                   outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")

                .CreateLogger();


            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // services.AddLogging(configure => configure.AddDebug());

            services.AddLogging(configure => configure.AddSimpleConsole(c =>
            {
                c.ColorBehavior = LoggerColorBehavior.Enabled;
                c.UseUtcTimestamp = false;
                c.TimestampFormat = "[yy-MM-dd HH:mm:ss ^^^]";
            }));

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            RegisterDependency(services);

            Container = services.BuildServiceProvider();

        }


        private static void RegisterDependency(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<TestJsonLogging>();
        }


        
        private static void TestLogFactory()
        {
            //works by itself, without external Ioc support since .Create does it all.
            // Crate log factory, not logger. This will create ServiceCollection, and MS log factory and ServiceProvider.
            // It will dispose itself which is dispose this ServiceProvider at the end because of "using".
            using var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("ConsoleApp1", LogLevel.Warning)
                    .AddSerilog(null, true);  //Does nothing since no sink
                //.AddConsole();  //works
            });

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("------ Seillog TestLogFactory no output ----------");
        }


        private static void TestLogFactory2()
        {

            Serilog.Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}] [{SourceContext}] {Message:lj} {NewLine}{Exception}")
                .CreateLogger();

            //works by itself, without external Ioc support since .Create does it all.
            // Crate log factory, not logger. This will create ServiceCollection, and MS log factory and ServiceProvider.
            // It will dispose itself which is dispose this ServiceProvider at the end because of "using".
            using var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("ConsoleApp1", LogLevel.Warning) //unless filter out
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddSerilog(null, true)
                    .AddConsole()  //this line move up or not, both works, it print twice and in the order list here
                    ;
            });

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("------ Seillog TestLogFactory global instance cfged so  there is output----------");
        }



    }
}



