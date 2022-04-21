using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck
{
    /* Things to find out:

    Do I need register health check class in ioc container? Yes, but need registered as self. This does not apply to TypeActivatedCheck which always created again.
    how often container call health end point
    best way to connect to couchbase
    best way to 

    */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddSingleton<TypeActivatedCheck, TypeActivatedCheck>();
            services.AddSingleton<MyHealthCheck, MyHealthCheck>();

            var builder = services.AddHealthChecks();

            builder.AddCheck("Lambda check 1",
                (CancellationToken t) => { return HealthCheckResult.Degraded("Lambda check 1"); });

            builder.AddTypeActivatedCheck<TypeActivatedCheck>("TypeActivatedCheck");
            builder.AddCheck<MyHealthCheck>("MyHealthCheck");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            //app.MapHealthChecks("/health"); // Not found
            app.UseHealthChecks("/health"); // from Microsoft.AspNetCore.Diagnostics.HealthChecks.dll

        }
    }
}
