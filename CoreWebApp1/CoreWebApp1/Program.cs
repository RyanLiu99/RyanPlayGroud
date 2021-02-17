using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoreWebApp1
{
    public class Program
  {
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args) //=> IHostBuilder
            .ConfigureWebHostDefaults(webBuilder =>     // => IHostBuilder
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build()  // => IHost
            .Run();  //  =>  void
    }

  }
}
