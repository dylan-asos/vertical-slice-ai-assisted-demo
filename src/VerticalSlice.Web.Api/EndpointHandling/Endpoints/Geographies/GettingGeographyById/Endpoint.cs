using Kommand.Abstractions;
using VerticalSlice.Web.Api.Contracts.Response;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Mappers;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Queries;
using VerticalSlice.Web.Api.OpenApi;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.GettingGeographyById;

internal static class GetGeographyByIdEndpoint
{
    internal static IEndpointRouteBuilder UseGetGeographyByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/geographies/{id:int}",
                async (
                    IMediator mediator,
                    int id,
                    CancellationToken ct = default
                ) =>
                {
                    GetGeographyByIdQueryResult result = await mediator.QueryAsync(
                        new GetGeographyByIdQuery { GeographyId = id }, ct);

                    if (result.Geography == null)
                    {
                        return Results.NotFound();
                    }

                    return Results.Ok(result.Geography.ToGeographySummary());
                })
            .Produces<GeographySummary>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetGeographyById")
            .AddVerticalSliceOpenApi();

        return endpoints;
    }
}
