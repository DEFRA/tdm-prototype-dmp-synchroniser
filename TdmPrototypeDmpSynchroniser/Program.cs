using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Data;
// using TdmPrototypeDmpSynchroniser.Endpoints;
using TdmPrototypeDmpSynchroniser.Services;
using TdmPrototypeDmpSynchroniser.Utils;
using FluentValidation;
using Serilog;
using TdmPrototypeDmpSynchroniser.Endpoints;

//-------- Configure the WebApplication builder------------------//

var builder = WebApplication.CreateBuilder(args);

// Grab environment variables
builder.Configuration.AddEnvironmentVariables("CDP");
builder.Configuration.AddEnvironmentVariables();

// Serilog
builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.With<LogLevelMapper>()
    .CreateLogger();
builder.Logging.AddSerilog(logger);

logger.Information("Starting application");

// Load certificates into Trust Store - Note must happen before Mongo and Http client connections 
TrustStore.SetupTrustStore(logger);

// Mongo
builder.Services.AddSingleton<IMongoDbClientFactory>(_ =>
    new MongoDbClientFactory(builder.Configuration.GetValue<string>("Mongo:DatabaseUri")!,
        builder.Configuration.GetValue<string>("Mongo:DatabaseName")!));

// our services
builder.Services.AddSingleton<EnvironmentVariables, EnvironmentVariables>();
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddSingleton<IBusService, BusService>();

// health checks
builder.Services.AddHealthChecks();

// http client
builder.Services.AddHttpClient();
builder.Services.AddHttpProxyClient(logger);

// swagger endpoints
if (builder.IsSwaggerEnabled())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (builder.IsSwaggerEnabled())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseDiagnosticEndpoints();
app.MapHealthChecks("/health");

app.Run();
