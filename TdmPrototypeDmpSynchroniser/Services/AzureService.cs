using System.Diagnostics.Tracing;
using Amazon.SecurityToken.Model;
using Azure.Core;
using Azure.Core.Diagnostics;
using Azure.Core.Pipeline;
using Azure.Identity;
using MongoDB.Driver;
using TdmPrototypeDmpSynchroniser.Config;

namespace TdmPrototypeDmpSynchroniser.Services;

public abstract class AzureService : BaseService
{
    protected readonly TokenCredential Credentials;
    protected readonly HttpClientTransport? Transport;
    
    protected AzureService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables, IHttpClientFactory? clientFactory = null) : base(
        loggerFactory, environmentVariables)
    {
        
        using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger(EventLevel.Verbose);

        if (EnvironmentVariables.AzureClientId != null)
        {
            Logger.LogInformation($"Creating azure credentials based on env vars for {EnvironmentVariables.AzureClientId}");
            Credentials = new EnvironmentCredential();
        }
        else
        {
            Logger.LogInformation($"Creating azure credentials using default creds because AZURE_CLIENT_ID env var not found.");
            Credentials = new DefaultAzureCredential();
        }

        if (clientFactory != null)
        {
            Transport = new HttpClientTransport(clientFactory.CreateClient("proxy"));    
        }
        
    }
}