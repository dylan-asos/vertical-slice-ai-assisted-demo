using Kommand.Abstractions;
using VerticalSlice.Web.Api.Contracts.Response;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Mappers;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Queries;
using VerticalSlice.Web.Api.OpenApi;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.GettingGeographies;

internal static class GetGeographiesEndpoint
{
    internal static IEndpointRouteBuilder UseGetGeographiesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/geographies",
                async (
                    IMediator mediator,
                    int page = 1,
                    int pageSize = 50,
                    string? searchTerm = null,
                    string? region = null,
                    string? subRegion = null,
                    CancellationToken ct = default
                ) =>
                {
                    GetAllGeographiesQuery query = new()
                    {
                        Page = page,
                        PageSize = pageSize,
                        SearchTerm = searchTerm,
                        Region = region,
                        SubRegion = subRegion,
                    };

                    GetAllGeographiesQueryResult result = await mediator.QueryAsync(query, ct);

                    return result.Geographies.ToPagedResponse(page, pageSize, result.TotalCount);
                })
            .Produces<PagedGeographyResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("GetGeographies")
            .AddVerticalSliceOpenApi();

        return endpoints;
    }
}
