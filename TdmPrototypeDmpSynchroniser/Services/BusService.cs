using System.Net;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;
using TdmPrototypeDmpSynchroniser.Utils;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BusService(
    ILoggerFactory loggerFactory,
    EnvironmentVariables environmentVariables,
    Proxy.IProxyConfig proxyConfig) : AzureService(loggerFactory, environmentVariables), IBusService
{
    public async Task<Status> CheckBusAsync()
    {
        Logger.LogInformation(
            $"Connecting to bus {EnvironmentVariables.DmpBusNamespace} : {EnvironmentVariables.DmpBusTopic}/{EnvironmentVariables.DmpBusSubscription}");

        var clientOptions = new ServiceBusClientOptions()
        {
            WebProxy = proxyConfig.UseProxy ? proxyConfig.Proxy : null,
            TransportType = ServiceBusTransportType.AmqpWebSockets,
            RetryOptions = new ServiceBusRetryOptions
            {
                TryTimeout = TimeSpan.FromSeconds(10), MaxRetries = 1
            },
            // Diagnostics = 
            // {
            //     IsLoggingContentEnabled = true,
            // }
        };
        
        var client = new ServiceBusClient(
            EnvironmentVariables.DmpBusNamespace,
            Credentials,
            clientOptions);

        var processor =
            client.CreateReceiver(EnvironmentVariables.DmpBusTopic, EnvironmentVariables.DmpBusSubscription);

        try
        {
            var messages = await processor.PeekMessagesAsync(100);

            return new Status()
            {
                Success = true, Description = String.Format("Connected. {0} bus messages found", messages.Count)
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return new Status() { Success = false, Description = ex.Message };
        }
        finally
        {
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }

    }
}