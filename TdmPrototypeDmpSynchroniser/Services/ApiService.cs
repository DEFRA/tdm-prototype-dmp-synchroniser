using MongoDB.Driver;

namespace TdmPrototypeDmpSynchroniser.Services;

public abstract class ApiService
{
    protected readonly ILogger Logger;

    protected ApiService(ILoggerFactory loggerFactory)
    {
        var loggerName = GetType().FullName ?? GetType().Name;
        Logger = loggerFactory.CreateLogger(loggerName);
    }
}