﻿using System.Diagnostics.Tracing;
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
    
    protected AzureService(ILoggerFactory loggerFactory, SynchroniserConfig config, IHttpClientFactory? clientFactory = null) : base(
        loggerFactory, config)
    {
        
        using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger(EventLevel.Verbose);

        if (Config.AzureClientId != null)
        {
            Logger.LogInformation($"Creating azure credentials based on config vars for {Config.AzureClientId}");
            Credentials =
                new ClientSecretCredential(Config.AzureTenantId, Config.AzureClientId, Config.AzureClientSecret);
            
            Logger.LogInformation($"Created azure credentials");
        }
        else
        {
            Logger.LogInformation($"Creating azure credentials using default creds because AZURE_CLIENT_ID env var not found.");
            Credentials = new DefaultAzureCredential();
            Logger.LogInformation($"Created azure default credentials");
        }

        if (clientFactory != null)
        {
            Transport = new HttpClientTransport(clientFactory.CreateClient("proxy"));    
        }
        
    }
}