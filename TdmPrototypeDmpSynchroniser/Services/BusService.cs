using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BusService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables) : ApiService(loggerFactory), IBusService
{   
    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Console.WriteLine($"Received: {body} from subscription.");

        // complete the message. messages is deleted from the subscription. 
        await args.CompleteMessageAsync(args.Message);
    }
    
    // handle any errors when receiving messages
    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
    
    public async Task<Status> CheckBusASync()
    {
        //https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-how-to-use-topics-subscriptions?tabs=passwordless
        
        Logger.LogInformation("Connecting to bus {0} : {1}/{2}", environmentVariables.DmpBusNamespace, environmentVariables.DmpBusTopic, environmentVariables.DmpBusSubscription);

        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets,
            RetryOptions = new ServiceBusRetryOptions
            {
                TryTimeout = TimeSpan.FromSeconds(10)  // This is the default value
            }
        };
        var client = new ServiceBusClient(
            environmentVariables.DmpBusNamespace,
            new DefaultAzureCredential(),
            clientOptions);

        var processor = client.CreateReceiver(environmentVariables.DmpBusTopic, environmentVariables.DmpBusSubscription);
    
        try
        {
            var messages = await processor.PeekMessagesAsync(100);
            
            return new Status() { Success = true, Description = String.Format("Connected. {0} bus messages found", messages.Count)  };
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