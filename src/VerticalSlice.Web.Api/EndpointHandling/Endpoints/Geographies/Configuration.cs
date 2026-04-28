using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.GettingGeographies;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.GettingGeographyById;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.UpsertingGeography;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies;

public static class Configuration
{
    public static IEndpointRouteBuilder UseGeographyEndpoints(this IEndpointRouteBuilder endpoints) =>
        endpoints
            .UseGetGeographiesEndpoint()
            .UseGetGeographyByIdEndpoint()
            .UseUpsertGeographyEndpoint();
}
