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
    SynchroniserConfig config,
    Proxy.IProxyConfig proxyConfig) : AzureService(loggerFactory, config), IBusService
{

    private ServiceBusClient CreateBusClient(string uri, int retries = 1, int timeout = 10)
    {
        Logger.LogInformation(
            $"Connecting to bus {uri} : {Config.DmpBusTopic}/{Config.DmpBusSubscription}");

        var clientOptions = new ServiceBusClientOptions()
        {
            WebProxy = proxyConfig.UseProxy ? proxyConfig.Proxy : null,
            TransportType = ServiceBusTransportType.AmqpWebSockets,
            RetryOptions = new ServiceBusRetryOptions { TryTimeout = TimeSpan.FromSeconds(10), MaxRetries = 0 },
        };

        return new ServiceBusClient(
            uri,
            Credentials,
            clientOptions);
    }

    public async Task<Status> CheckBusAsync()
    {
        return await CheckBusAsync(Config.DmpBusNamespace);
    }

    public async Task<Status> CheckBusAsync(string uri)
    {   
        try
        {
            Extensions.AssertIsNotNull(uri);

            await using (var client = CreateBusClient(uri, 0, 5))
            {
                await using (var processor =
                             client.CreateReceiver(Config.DmpBusTopic, Config.DmpBusSubscription))
                {
                    var messages = await processor.PeekMessagesAsync(100);

                    return new Status()
                    {
                        Success = true,
                        Description = String.Format("Connected. {0} bus messages found", messages.Count)
                    };
                }
            }

        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return new Status() { Success = false, Description = ex.Message };
        }
    }
}