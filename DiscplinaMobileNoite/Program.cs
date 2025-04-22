//using DiscplinaMobileNoite.Extensions;
//using DiscplinaMobileNoite.Extensions.ExtensionsLogs;
//using DiscplinaMobileNoite.Infrastracture.Connections;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers();
//builder.Services.AddApplicationServices(builder.Configuration);

//LogExtension.InitializeLogger();

//var loggerSerialLog = LogExtension.GetLogger();

//loggerSerialLog.Information("Logging initialized.");

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
//    });
//}

//app.UseCors("CorsPolicy");

//app.UseAuthorization();

//app.MapControllers();

//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;

//try
//{
//    var context = services.GetRequiredService<DataContext>();
//    context.Database.Migrate();
//}
//catch (Exception ex)
//{
//    var logger = services.GetRequiredService<ILogger<Program>>();
//    logger.LogError(ex, "An error occured during migration!");
//}

//app.UseMiddleware<ExceptionMiddleware>();

//app.Run();

using DiscplinaMobileNoite.Extensions;
using DiscplinaMobileNoite.Extensions.ExtensionsLogs;
using DiscplinaMobileNoite.Infrastracture.Connections;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Permitir que o Kestrel escute em qualquer IP (acesso de rede local)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
    options.ListenAnyIP(5001, listen => listen.UseHttps()); // HTTPS, se necessário
});

// 🔹 Configurar CORS para permitir acesso externo
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🔹 Serviços e logs
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

LogExtension.InitializeLogger();
var loggerSerialLog = LogExtension.GetLogger();
loggerSerialLog.Information("Logging initialized.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

// 🔹 Ambiente de desenvolvimento: habilitar Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
    });
}

// 🔹 Aplicar políticas de CORS
app.UseCors("CorsPolicy");

// 🔹 Middleware de tratamento de exceções
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("CorsPolicy");

// 🔹 Autorização e rotas
app.UseAuthorization();
app.MapControllers();

// 🔹 Rodar migração automática do banco
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
