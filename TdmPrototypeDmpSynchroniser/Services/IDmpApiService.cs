using TdmPrototypeBackend.Types;
using TdmPrototypeDmpSynchroniser.Models;

namespace TdmPrototypeDmpSynchroniser.Services;

public interface IDmpApiService
{
    public void UpsertMovement(Movement movement);
}