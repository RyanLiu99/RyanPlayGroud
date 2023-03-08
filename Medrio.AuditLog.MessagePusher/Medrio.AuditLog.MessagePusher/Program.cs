
using Google.Api;
using Medrio.AuditLog.MessagePusher;
using Medrio.Infrastructure.Ioc;
using Microsoft.AspNetCore.Server.Kestrel.Core;


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

// Add services to the container.
builder.Services.AddControllers();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

app.Services.SetUpIocAdapter();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
