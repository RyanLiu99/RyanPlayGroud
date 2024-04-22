using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/", async Task<string> () =>
{
    var dateTime = DateTime.Now;
    var guidKey = Guid.NewGuid().ToString();
    var guidValue = Guid.NewGuid();
    var name = "name";
    var age = 21;
    CallContext.LogicalSetData("DateTime", dateTime);
    CallContext.LogicalSetData(guidKey, guidValue);
    CallContext.LogicalSetData("Name", name);
    CallContext.LogicalSetData("Age", age);

    await Task.Delay(20);

    var dateTimeBack = CallContext.LogicalGetData("DateTime") as DateTime?;
    var guidBack = CallContext.LogicalGetData(guidKey) as Guid?;
    var nameBack = CallContext.LogicalGetData("Name") as string;
    var ageBack = CallContext.LogicalGetData("Age") as int?;

    Trace.Assert(dateTimeBack == dateTime);
    Trace.Assert(guidBack == guidBack.GetValueOrDefault() );
    Trace.Assert(nameBack == name);
    Trace.Assert(ageBack == age);

    return dateTime.ToString();
})
.WithName("root")
.WithOpenApi();

app.Run();