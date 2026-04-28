using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Audits;
using VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints;

public static class Configuration
{
    public static void MapVerticalSliceEndpoints(this RouteGroupBuilder builder)
    {
        builder.UseAuditEndpoints();
        builder.UseGeographyEndpoints();
    }
}
