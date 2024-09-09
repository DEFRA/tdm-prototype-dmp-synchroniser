using Amazon.SecurityToken.Model;
using Azure.Core;
using Azure.Core.Diagnostics;
using Azure.Identity;
using MongoDB.Driver;
using TdmPrototypeDmpSynchroniser.Config;

namespace TdmPrototypeDmpSynchroniser.Services;

public abstract class AzureService : BaseService
{
    protected readonly TokenCredential Credentials;

    protected AzureService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables) : base(
        loggerFactory, environmentVariables)
    {
        using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

        if (environmentVariables.AzureClientId != null)
        {
            Logger.LogInformation($"Creating azure credentials based on env vars for {environmentVariables.AzureClientId}");
            Credentials = new EnvironmentCredential();
        }
        else
        {
            Logger.LogInformation($"Creating azure credentials using default creds because AZURE_CLIENT_ID env var not found.");
            Credentials = new DefaultAzureCredential();
        }
    }
}