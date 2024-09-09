using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public interface IWebService
{
    public Task<Status> CheckTradeApiAsync();
    public Task<Status> CheckTradeApiInternalAsync();
    public Task<Status> CheckGoogleAsync();
    
}