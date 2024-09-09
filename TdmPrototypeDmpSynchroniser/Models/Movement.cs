
namespace TdmPrototypeBackend.Types;

// Temporary file whilst figuring out to share the type library between services

public class MovementItem
{
     public string CustomsProcedureCode { get; set; } = default!;
}

public class Movement
{
    public string Id { get; set; } = default!;
    public MovementItem[] Items { get; set; } = default!;
}