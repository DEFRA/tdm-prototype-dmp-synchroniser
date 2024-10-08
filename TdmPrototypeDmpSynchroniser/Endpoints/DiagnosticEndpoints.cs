﻿using TdmPrototypeDmpSynchroniser.Models;
using TdmPrototypeDmpSynchroniser.Services;
using FluentValidation;
using FluentValidation.Results;
using TdmPrototypeDmpSynchroniser.Config;

// using MongoDB.Bson;

namespace TdmPrototypeDmpSynchroniser.Endpoints;

public static class DiagnosticEndpoints
{
    private const string BaseRoute = "diagnostics";

    public static void UseDiagnosticEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(BaseRoute + "/bus", GetBusDiagnosticAsync);
        app.MapGet(BaseRoute + "/blob", GetBlobDiagnosticAsync);
        app.MapGet(BaseRoute + "/bus-internal", GetBusInternalDiagnosticAsync);
        app.MapGet(BaseRoute + "/blob-internal", GetBlobInternalDiagnosticAsync);
        app.MapGet(BaseRoute + "/tradeapi", GetTradeApiDiagnosticAsync);
        app.MapGet(BaseRoute + "/tradeapi-internal", GetTradeApiInternalDiagnosticAsync);
        app.MapGet(BaseRoute + "/google", GetTradeApiDiagnosticAsync);
    }

    private static async Task<IResult> GetBusDiagnosticAsync(
        IBusService service)
    {
        var result = await service.CheckBusAsync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }

    private static async Task<IResult> GetBusInternalDiagnosticAsync(
        IBusService service, SynchroniserConfig config)
    {
        var result = await service.CheckBusAsync(config.DmpBusPrivateEndpoint!);
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
    
    private static async Task<IResult> GetBlobDiagnosticAsync(
        IBlobService service)
    {
        var result = await service.CheckBlobAsync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
    
    private static async Task<IResult> GetBlobInternalDiagnosticAsync(
        IBlobService service, SynchroniserConfig config)
    {
        var result = await service.CheckBlobAsync(config.DmpBlobPrivateEndpoint!);
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
    
    private static async Task<IResult> GetTradeApiDiagnosticAsync(
        IWebService service)
    {
        var result = await service.CheckTradeApiAsync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
    
    private static async Task<IResult> GetTradeApiInternalDiagnosticAsync(
        IWebService service)
    {
        var result = await service.CheckTradeApiInternalAsync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
    
    private static async Task<IResult> GetGoogleDiagnosticAsync(
        IWebService service)
    {
        var result = await service.CheckGoogleAsync();
        Console.WriteLine(result.ToJson());
        if (result.Success)
        {
            return Results.Ok(result);    
        }
        return Results.Conflict(result);
    }
}