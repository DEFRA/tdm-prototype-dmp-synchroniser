

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using Azure;
using Azure.Storage.Blobs.Models;
using MongoDB.Driver.Linq;
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
            foreach (BlobItem item in result.Take(5))
            {
                if (item.Properties.ContentLength is 0)
                {
                    Console.WriteLine($"{item.Name} is a virtual folder.");
                }
                else
                {
                    dmpService.UpsertMovement(await ConvertMovement(item));
                    itemCount++;
                }

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

    private async Task<Movement> ConvertMovement(BlobItem item)
    {
        var blob = await blobService.GetBlobAsync(item.Name);
        
        Logger.LogInformation(blob.Content.ToString());
        // var json = JsonObject.Parse(blob.Content.ToDynamicFromJson());
        var json = blob.Content.ToDynamicFromJson();
        // Logger.LogInformation(json);
        
        return new Movement() { Id = "" };;
    }

}