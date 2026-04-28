using Kommand.Abstractions;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Queries;

public class GetGeographyByIdQuery : IQuery<GetGeographyByIdQueryResult>
{
    public int GeographyId { get; set; }
}
