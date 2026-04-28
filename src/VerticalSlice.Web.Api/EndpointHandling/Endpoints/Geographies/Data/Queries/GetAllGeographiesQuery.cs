using Kommand.Abstractions;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Queries;

public class GetAllGeographiesQuery : IQuery<GetAllGeographiesQueryResult>
{
    public string? SearchTerm { get; set; }
    public string? Region { get; set; }
    public string? SubRegion { get; set; }

    // Pagination parameters
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
