using Kommand.Abstractions;
using VerticalSlice.Web.Api.Contracts.Response;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Commands;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Mappers;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Queries;
using VerticalSlice.Web.Api.OpenApi;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.UpsertingGeography;

internal static class UpsertGeographyEndpoint
{
    internal static IEndpointRouteBuilder UseUpsertGeographyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPut(
                "/geographies",
                async (
                    IMediator mediator,
                    UpsertGeographyCommand command,
                    CancellationToken ct = default
                ) =>
                {
                    UpsertGeographyCommandResult result = await mediator.SendAsync(command, ct);

                    GetGeographyByIdQueryResult queryResult = await mediator.QueryAsync(
                        new GetGeographyByIdQuery { GeographyId = result.GeographyId }, ct);

                    if (queryResult.Geography == null)
                    {
                        return Results.Problem("Failed to retrieve the saved geography.");
                    }

                    GeographySummary summary = queryResult.Geography.ToGeographySummary();

                    return result.IsNew
                        ? Results.Created($"/api/v1/geographies/{result.GeographyId}", summary)
                        : Results.Ok(summary);
                })
            .Produces<GeographySummary>(StatusCodes.Status200OK)
            .Produces<GeographySummary>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("UpsertGeography")
            .AddVerticalSliceOpenApi();

        return endpoints;
    }
}
