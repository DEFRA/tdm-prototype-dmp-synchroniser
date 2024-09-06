

using System.Diagnostics.CodeAnalysis;
using Azure.Storage.Blobs.Models;
using TdmPrototypeBackend.Types;
using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;


namespace TdmPrototypeDmpSynchroniser.Services;

public class SyncService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables, IBlobService blobService, IDmpApiService dmpService)
    : BaseService(loggerFactory, environmentVariables), ISyncService
{
    
    public async Task<Status> SyncMovements()
    {
        try
        {
            var result = await blobService.GetResourcesAsync("RAW/ALVS/");
            
            var itemCount = 0;
            foreach (BlobItem item in result)
            {
                dmpService.UpsertMovement(ConvertMovement(item));
                itemCount++;
            }
            
            return new Status()
            {
                Success = true, Description = String.Format("Connected. {0} items upserted to DMP API", itemCount)
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return new Status() { Success = false, Description = ex.Message };
        }
    }

    private Movement ConvertMovement(BlobItem item)
    {
        return new Movement() { Id = "" };
    }
}