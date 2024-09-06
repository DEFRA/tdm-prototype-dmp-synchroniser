using TdmPrototypeDmpSynchroniser.Models;
using TdmPrototypeDmpSynchroniser.Services;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Bson;

namespace TdmPrototypeDmpSynchroniser.Endpoints;

public static class DiagnosticEndpoints
{
    private const string ContentType = "application/json";
    private const string Tag = "Books";
    private const string BaseRoute = "diagnostics";

    public static void UseDiagnosticEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(BaseRoute + "/bus", GetBusDiagnosticAsync);
        app.MapGet(BaseRoute + "/blob", GetBlobDiagnosticAsync);
    }

    private static async Task<IResult> GetBusDiagnosticAsync(
        IBusService service, string? searchTerm)
    {
        Console.WriteLine("GetBusDiagnosticAsync");
        var result = service.CheckBusASync();
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
        Console.WriteLine("GetBlobDiagnosticAsync");
        var result = await service.CheckBlobASync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
}