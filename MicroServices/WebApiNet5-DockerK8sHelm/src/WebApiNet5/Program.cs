using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace WebApiNet5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Program start at " + DateTime.Now);
            Console.WriteLine("------------ Go to /Index?q=aa  or /swagger/-------------------");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
