using DiagnosKit.Core.Logging;
using DiagnosKit.Core.Logging.Contracts;
using DiagnosKit.Core.Middlewares;
using KwikNesta.Infrastruture.Svc.API.Extensions;

SerilogBootstrapper.UseBootstrapLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
    .RegisterServices(builder.Configuration, builder.Environment.ApplicationName);

if (!builder.Environment.IsDevelopment())
{
    builder.Host.ConfigureESSink(builder.Configuration);
}

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.UseDiagnosKitExceptionHandler(logger);
app.RegisterMiddlewares(builder.Configuration);

app.Run();
