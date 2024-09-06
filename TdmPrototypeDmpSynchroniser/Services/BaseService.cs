using Amazon.SecurityToken.Model;
using Azure.Core;
using Azure.Identity;
using MongoDB.Driver;
using TdmPrototypeDmpSynchroniser.Config;

namespace TdmPrototypeDmpSynchroniser.Services;

public abstract class BaseService
{
    protected readonly ILogger Logger;

    protected BaseService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables)
    {
        var loggerName = GetType().FullName ?? GetType().Name;
        Logger = loggerFactory.CreateLogger(loggerName);
    }
}