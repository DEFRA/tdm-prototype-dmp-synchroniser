using Amazon.SecurityToken.Model;
using Azure.Core;
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
        Credentials = environmentVariables.AzureClientId == null
            ? new DefaultAzureCredential()
            : new EnvironmentCredential();
    }
}