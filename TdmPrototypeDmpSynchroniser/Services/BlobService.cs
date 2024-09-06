using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TdmPrototypeDmpSynchroniser.Config;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public class BlobService(ILoggerFactory loggerFactory, EnvironmentVariables environmentVariables)
    : AzureService(loggerFactory, environmentVariables), IBlobService
{
    public async Task<Status> CheckBlobASync()
    {
        // https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet?tabs=visual-studio%2Cmanaged-identity%2Croles-azure-portal%2Csign-in-azure-cli%2Cidentity-visual-studio&pivots=blob-storage-quickstart-scratch

        Logger.LogInformation("Connecting to blob storage {0} : {1}", environmentVariables.DmpBlobUri,
            environmentVariables.DmpBlobContainer);
        try
        {

            var blobServiceClient = new BlobServiceClient(
                new Uri(environmentVariables.DmpBlobUri),
                Credentials);

            // Create the container and return a container client object
            BlobContainerClient containerClient =
                blobServiceClient.GetBlobContainerClient(environmentVariables.DmpBlobContainer);

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
}