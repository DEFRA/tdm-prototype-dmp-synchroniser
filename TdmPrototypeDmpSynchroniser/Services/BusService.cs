using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BusService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables) : AzureService(loggerFactory, environmentVariables), IBusService
{
    public async Task<Status> CheckBusASync()
    {
        Logger.LogInformation("Connecting to bus {0} : {1}/{2}", environmentVariables.DmpBusNamespace,
            environmentVariables.DmpBusTopic, environmentVariables.DmpBusSubscription);

        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets,
            RetryOptions = new ServiceBusRetryOptions { TryTimeout = TimeSpan.FromSeconds(10) }
        };
        var client = new ServiceBusClient(
            environmentVariables.DmpBusNamespace,
            Credentials,
            clientOptions);

        var processor =
            client.CreateReceiver(environmentVariables.DmpBusTopic, environmentVariables.DmpBusSubscription);

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