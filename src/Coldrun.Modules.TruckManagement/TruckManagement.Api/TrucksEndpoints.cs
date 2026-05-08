using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Coldrun.Modules.TruckManagement.Api;

public static class TrucksEndpoints
{
    public static IEndpointRouteBuilder MapTrucksEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/v1/trucks")
            .WithTags("Trucks");

        group.MapGet("/", SearchTrucks);
        group.MapPost("/", CreateTruck);
        group.MapGet("/{code}", GetTruck);
        group.MapPatch("/{code}", UpdateTruckDetails);
        group.MapDelete("/{code}", DeleteTruck);

        group.MapGet("/{code}/status", GetTruckStatus);
        group.MapPut("/{code}/status", ChangeTruckStatus);

        return app;
    }

    private static async Task<IResult> SearchTrucks(
        CancellationToken cancellationToken)
    {
        return Results.Ok();
    }

    private static async Task<IResult> CreateTruck(
        CreateTruckRequest request,
        CancellationToken cancellationToken)
    {
        return Results.Created($"/api/v1/trucks/{request.Code}", null);
    }

    private static async Task<IResult> GetTruck(
        string code,
        CancellationToken cancellationToken)
    {
        return Results.Ok();
    }

    private static async Task<IResult> UpdateTruckDetails(
        string code,
        UpdateTruckDetailsRequest request,
        CancellationToken cancellationToken)
    {
        return Results.Ok();
    }

    private static async Task<IResult> DeleteTruck(
        string code,
        CancellationToken cancellationToken)
    {
        return Results.NoContent();
    }

    private static async Task<IResult> GetTruckStatus(
        string code,
        CancellationToken cancellationToken)
    {
        return Results.Ok();
    }

    private static async Task<IResult> ChangeTruckStatus(
        string code,
        ChangeTruckStatusRequest request,
        CancellationToken cancellationToken)
    {
        return Results.Ok();
    }
}
