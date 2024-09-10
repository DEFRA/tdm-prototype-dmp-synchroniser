using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TdmPrototypeBackend.Types;
using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class DmpApiService: BaseService, IDmpApiService
{
    public DmpApiService(ILoggerFactory loggerFactory, SynchroniserConfig config) : base(loggerFactory, config)
    {
        Logger.LogInformation("Connecting to Dmp API {0}", config.TdmBackendApiUri);
    }
    public async void UpsertMovement(Movement movement)
    {
        try
        {
            

            
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            throw;
        }

    }
}