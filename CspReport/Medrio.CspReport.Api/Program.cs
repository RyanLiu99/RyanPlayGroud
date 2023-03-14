using Medrio.CspReport;
using Medrio.Infrastructure.Ioc;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AutoRegisterServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(Constants.Cors_CspReportPolicy,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .WithMethods("POST");
        });
});

builder.Services.AddControllers();

var app = builder.Build();
app.Services.SetUpIocAdapter();

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();
app.MapControllers();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    //push last few bits
});

#if DEBUG
//for local test
app.Run();

#else
//for run in GCP in Release mode
//This is how GCP cloud run works. GCP will inject this env variable. The value is configurable at deployment time
//GCP cloud run will also provides https endpoint and SSL certificate, no need binding here.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var url = $"http://0.0.0.0:{port}";
app.Run(url);
#endif