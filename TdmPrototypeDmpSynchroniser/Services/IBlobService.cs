using Azure.Storage.Blobs.Models;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public interface IBlobService
{
    public Task<Status> CheckBlobAsync();
    public Task<Status> CheckBlobAsync(string uri);
    public Task<IEnumerable<BlobItem>> GetResourcesAsync(string prefix);
    public Task<BlobDownloadResult> GetBlobAsync(string path);
    
}