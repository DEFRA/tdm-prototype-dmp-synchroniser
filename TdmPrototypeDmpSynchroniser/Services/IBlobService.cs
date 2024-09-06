using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public interface IBlobService
{
    public Task<Status> CheckBlobASync();
    
}