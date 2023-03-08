using Medrio.CspReport;
using Medrio.Infrastructure.Ioc;

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

app.Run();
