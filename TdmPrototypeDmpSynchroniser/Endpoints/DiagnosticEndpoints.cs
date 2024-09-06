using TdmPrototypeDmpSynchroniser.Models;
using TdmPrototypeDmpSynchroniser.Services;
using FluentValidation;
using FluentValidation.Results;
// using MongoDB.Bson;

namespace TdmPrototypeDmpSynchroniser.Endpoints;

public static class DiagnosticEndpoints
{
    private const string BaseRoute = "diagnostics";

    public static void UseDiagnosticEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(BaseRoute + "/bus", GetBusDiagnosticAsync);
        app.MapGet(BaseRoute + "/blob", GetBlobDiagnosticAsync);
        app.MapGet(BaseRoute + "/tradeapi", GetTradeApiDiagnosticAsync);
    }

    private static async Task<IResult> GetBusDiagnosticAsync(
        IBusService service, string? searchTerm)
    {
        var result = await service.CheckBusASync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
    
    private static async Task<IResult> GetBlobDiagnosticAsync(
        IBlobService service, string? searchTerm)
    {
        var result = await service.CheckBlobAsync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
    
    
    private static async Task<IResult> GetTradeApiDiagnosticAsync(
        IWebService service, string? searchTerm)
    {
        var result = await service.CheckTradeApiAsync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
}