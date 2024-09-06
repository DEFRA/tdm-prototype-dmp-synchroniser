using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public interface ISyncService
{
    public Task<Status> SyncMovements();
    
}