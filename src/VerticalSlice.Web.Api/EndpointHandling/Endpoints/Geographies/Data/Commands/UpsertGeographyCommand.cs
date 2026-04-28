using Kommand.Abstractions;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Commands;

public class UpsertGeographyCommand : ICommand<UpsertGeographyCommandResult>
{
    public int? GeographyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string? GeoCodes { get; set; }
    public string? Region { get; set; }
    public string? SubRegion { get; set; }
    public string? Capital { get; set; }
    public long? Population { get; set; }
    public double? AreaKm2 { get; set; }
}
