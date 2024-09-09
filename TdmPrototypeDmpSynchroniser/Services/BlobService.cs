using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BlobService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables, IHttpClientFactory clientFactory)
    : AzureService(loggerFactory, environmentVariables, clientFactory), IBlobService
{
    private BlobContainerClient CreateBlobClient()
    {
        var options = new BlobClientOptions
        {
            Transport = Transport,
            Retry =
            {
                MaxRetries = 1,
                NetworkTimeout = TimeSpan.FromMinutes(10)
            }
        };
        
        var blobServiceClient = new BlobServiceClient(
            new Uri(EnvironmentVariables.DmpBlobUri),
            Credentials,
            options);

        return blobServiceClient.GetBlobContainerClient(EnvironmentVariables.DmpBlobContainer);
    }
    public async Task<Status> CheckBlobAsync()
    {
        Logger.LogInformation("Connecting to blob storage {0} : {1}", EnvironmentVariables.DmpBlobUri,
            EnvironmentVariables.DmpBlobContainer);
        try
        {
            var containerClient = CreateBlobClient();
            
            Logger.LogInformation("Getting blob folders...");
            var folders = containerClient.GetBlobsByHierarchyAsync(prefix: "RAW/", delimiter: "/");

            var itemCount = 0;
            await foreach (BlobHierarchyItem blobItem in folders)
            {
                Console.WriteLine("\t" + blobItem.Prefix);
                itemCount++;
            }

            return new Status()
            {
                Success = true, Description = String.Format("Connected. {0} blob folders found in RAW", itemCount)
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return new Status() { Success = false, Description = ex.Message };
        }

    }

    public async Task<IEnumerable<BlobItem>> GetResourcesAsync(string prefix)
    {
        Logger.LogInformation("Connecting to blob storage {0} : {1}", EnvironmentVariables.DmpBlobUri,
            EnvironmentVariables.DmpBlobContainer);
        try
        {
            var containerClient = CreateBlobClient();

            Logger.LogInformation("Getting blob files from {0}...", prefix);
            var itemCount = 0;
            // var folders = containerClient.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/");
            //
            // await foreach (BlobHierarchyItem blobItem in folders)
            // {
            //     Console.WriteLine("\t" + blobItem.Prefix);
            //     itemCount++;
            // }

            var files = containerClient.GetBlobsAsync(prefix: prefix);
            var output = new List<BlobItem>();
            
            await foreach (BlobItem item in files)
            {
                Console.WriteLine("\t" + item.Name);
                itemCount++;
                output.Add(item);
            }

            return output;
            // return new Status() { Success = true, Description = String.Format("Done. {0} items synchronised", itemCount) };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            throw;
        }

    }
}