namespace VerticalSlice.Web.Api.Model;

public class Geography
{
    public int GeographyId { get; set; }

    /// <summary>
    ///     The full name of the geography (e.g., United States, France)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     The ISO 3166-1 alpha-2 short code (e.g., US, FR)
    /// </summary>
    public string ShortCode { get; set; } = string.Empty;

    /// <summary>
    ///     Geographic coordinates as JSON (e.g., { "latitude": 38.9, "longitude": -77.0 })
    /// </summary>
    public string? GeoCodes { get; set; }

    /// <summary>
    ///     The major world region (e.g., Americas, Europe, Asia, Africa, Oceania)
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    ///     The sub-region within the major region (e.g., Northern America, Western Europe)
    /// </summary>
    public string? SubRegion { get; set; }

    /// <summary>
    ///     The capital city of the geography
    /// </summary>
    public string? Capital { get; set; }

    /// <summary>
    ///     Approximate population count
    /// </summary>
    public long? Population { get; set; }

    /// <summary>
    ///     Total area in square kilometres
    /// </summary>
    public double? AreaKm2 { get; set; }
}
