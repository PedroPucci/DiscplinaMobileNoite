using DiscplinaMobileNoite.Extensions;
using DiscplinaMobileNoite.Extensions.ExtensionsLogs;
using DiscplinaMobileNoite.Infrastracture.Connections;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Kestrel escutando na porta 5000
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
});

// 🔹 CORS liberado
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

// 🔹 Logs
LogExtension.InitializeLogger();
var loggerSerialLog = LogExtension.GetLogger();
loggerSerialLog.Information("Logging initialized.");

var app = builder.Build();

// 🔹 Ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
    });
}

app.UseRouting(); // ✅ importante em versões < .NET 7

// 🔹 CORS aplicado uma vez
//app.UseCors("CorsPolicy");
app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("Access-Control-Allow-Origin", "*");
    context.Response.Headers.TryAdd("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
    context.Response.Headers.TryAdd("Access-Control-Allow-Headers", "Content-Type, Authorization");

    if (context.Request.Method == HttpMethods.Options)
    {
        context.Response.StatusCode = 204;
        await context.Response.CompleteAsync(); // 🔥 ESSENCIAL
        return;
    }

    await next.Invoke();
});

// 🔹 Middleware customizado
app.UseMiddleware<ExceptionMiddleware>();

// 🔹 Autorização e endpoints
app.UseAuthorization();
app.MapControllers();

// 🔹 Migrations
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration!");
}

app.Run();
