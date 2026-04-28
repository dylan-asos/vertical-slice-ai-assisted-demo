using VerticalSlice.Web.Api.Contracts.Response;
using VerticalSlice.Web.Api.Model;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Mappers;

/// <summary>
///     Mapper for converting Geography entities to response DTOs
/// </summary>
public static class GeographyMapper
{
    /// <summary>
    ///     Maps a Geography entity to GeographySummary
    /// </summary>
    public static GeographySummary ToGeographySummary(this Geography geography)
    {
        if (geography == null)
        {
            throw new ArgumentNullException(nameof(geography));
        }

        return new GeographySummary
        {
            GeographyId = geography.GeographyId,
            Name = geography.Name,
            ShortCode = geography.ShortCode,
            GeoCodes = geography.GeoCodes,
            Region = geography.Region,
            SubRegion = geography.SubRegion,
            Capital = geography.Capital,
            Population = geography.Population,
            AreaKm2 = geography.AreaKm2,
        };
    }

    /// <summary>
    ///     Maps a collection of Geography entities to GeographySummary collection
    /// </summary>
    public static IEnumerable<GeographySummary> ToGeographySummaries(this IEnumerable<Geography> geographies)
    {
        if (geographies == null)
        {
            throw new ArgumentNullException(nameof(geographies));
        }

        return geographies.Select(ToGeographySummary);
    }

    /// <summary>
    ///     Creates a paged response from geographies and pagination parameters
    /// </summary>
    public static PagedGeographyResponse ToPagedResponse(
        this IEnumerable<Geography> geographies,
        int page,
        int pageSize,
        int totalItems)
    {
        if (geographies == null)
        {
            throw new ArgumentNullException(nameof(geographies));
        }

        IEnumerable<GeographySummary> summaries = geographies.ToGeographySummaries();
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedGeographyResponse
        {
            Data = summaries,
            Pagination = new PaginationMetadata
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            }
        };
    }
}
