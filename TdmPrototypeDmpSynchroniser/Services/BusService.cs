using TdmPrototypeDmpSynchroniser.Data;
using TdmPrototypeDmpSynchroniser.Models;
using MongoDB.Driver;
using TdmPrototypeDmpSynchroniser.Data;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BusService(ILoggerFactory loggerFactory) : ApiService(loggerFactory), IBusService
{   
    public Status CheckBusASync()
    {
        // https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues?tabs=connection-string
        
        return new Status() { Success = true, Description = "Connected" };
    }
}