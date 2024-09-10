using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BlobService(ILoggerFactory loggerFactory, SynchroniserConfig config, IHttpClientFactory clientFactory)
    : AzureService(loggerFactory, config, clientFactory), IBlobService
{
    private BlobContainerClient CreateBlobClient(string uri, int retries = 1, int timeout = 10)
    {
        var options = new BlobClientOptions
        {
            Transport = Transport!,
            Retry =
            {
                MaxRetries = retries,
                NetworkTimeout = TimeSpan.FromSeconds(timeout)
            },
            Diagnostics = 
            {
                IsLoggingContentEnabled = true,
                IsLoggingEnabled = true
            }
        };
        
        var blobServiceClient = new BlobServiceClient(
            new Uri(uri),
            Credentials,
            options);

        var containerClient = blobServiceClient.GetBlobContainerClient(Config.DmpBlobContainer);
        
        return containerClient;
    }

    public async Task<Status> CheckBlobAsync()
    {
        return await CheckBlobAsync(Config.DmpBlobUri);
    }
    
    public async Task<Status> CheckBlobAsync(string uri)
    {
        Logger.LogInformation("Connecting to blob storage {0} : {1}", uri,
            Config.DmpBlobContainer);
        try
        {
            var containerClient = CreateBlobClient(uri, 0, 5);
            
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
        Logger.LogInformation("Connecting to blob storage {0} : {1}", Config.DmpBlobUri,
            Config.DmpBlobContainer);
        try
        {
            var containerClient = CreateBlobClient(Config.DmpBlobUri);

            Logger.LogInformation("Getting blob files from {0}...", prefix);
            var itemCount = 0;
            
            var files = containerClient.GetBlobsAsync(prefix: prefix);
            var output = new List<BlobItem>();
            
            await foreach (BlobItem item in files)
            {
                Console.WriteLine("\t" + item.Name);
                itemCount++;
                output.Add(item);
            }

            return output;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            throw;
        }

    }
    
    public async Task<BlobDownloadResult> GetBlobAsync(string path)
    {
        Logger.LogInformation(
            $"Downloading blob {path} from blob storage {Config.DmpBlobUri} : {Config.DmpBlobContainer}");
        try
        {
            var containerClient = CreateBlobClient(Config.DmpBlobUri);

            var blobClient = containerClient.GetBlobClient(path);

            var content = await blobClient.DownloadContentAsync();
            
            // content.Value.Content.
            return content.Value;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            throw;
        }

    }
}