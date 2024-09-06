using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BlobService(ILoggerFactory loggerFactory) : ApiService(loggerFactory), IBlobService
{   
    public async Task<Status> CheckBlobASync()
    {
        // https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet?tabs=visual-studio%2Cmanaged-identity%2Croles-azure-portal%2Csign-in-azure-cli%2Cidentity-visual-studio&pivots=blob-storage-quickstart-scratch
        // TODO: Replace <storage-account-name> with your actual storage account name
        var blobUrl = Environment.GetEnvironmentVariable("DMP_BLOB_STORAGE_URL")!;
        var blobContainer = Environment.GetEnvironmentVariable("DMP_BLOB_STORAGE_CONTAINER")!;
        
        Console.WriteLine("Connecting to {0} : {1}", blobUrl, blobContainer);
        try
        {
            
            var blobServiceClient = new BlobServiceClient(
                new Uri("https://" + blobUrl),
                new DefaultAzureCredential());
        
            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainer);
            
            Console.WriteLine("Getting blob folders...");
            var folders = containerClient.GetBlobsByHierarchyAsync(prefix: "RAW/", delimiter: "/");

            var itemCount = 0;
            await foreach (BlobHierarchyItem blobItem in folders)
            {
                Console.WriteLine("\t" + blobItem.Prefix);
                itemCount++;
            }
            
            // Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            // await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            // {
            //     Console.WriteLine("\t" + blobItem.Name);
            // }
            return new Status() { Success = true, Description = String.Format("Connected. {0} blob folders found in RAW", itemCount) };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Status() { Success = false, Description = ex.Message };
        }
        
    }
}