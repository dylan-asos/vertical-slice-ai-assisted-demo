using Kommand.Abstractions;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Web.Api.Data;
using VerticalSlice.Web.Api.Model;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Queries;

public class GetAllGeographiesQueryResult
{
    public IEnumerable<Geography> Geographies { get; set; } = [];
    public int TotalCount { get; set; }
}

public class GetAllGeographiesQueryHandler(VerticalSliceDataContext dataContext)
    : IQueryHandler<GetAllGeographiesQuery, GetAllGeographiesQueryResult>
{
    private readonly VerticalSliceDataContext _dataContext =
        dataContext ?? throw new ArgumentNullException(nameof(dataContext));

    public async Task<GetAllGeographiesQueryResult> HandleAsync(GetAllGeographiesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Geography> baseQuery = _dataContext.Geographies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Region))
        {
            string regionLower = request.Region.ToLower();
            baseQuery = baseQuery.Where(g => g.Region != null && g.Region.ToLower().Contains(regionLower));
        }

        if (!string.IsNullOrWhiteSpace(request.SubRegion))
        {
            string subRegionLower = request.SubRegion.ToLower();
            baseQuery = baseQuery.Where(g => g.SubRegion != null && g.SubRegion.ToLower().Contains(subRegionLower));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            string searchTerm = request.SearchTerm.ToLower();
            baseQuery = baseQuery.Where(g =>
                g.Name.ToLower().Contains(searchTerm) ||
                g.ShortCode.ToLower().Contains(searchTerm) ||
                (g.Capital ?? string.Empty).ToLower().Contains(searchTerm) ||
                (g.Region ?? string.Empty).ToLower().Contains(searchTerm));
        }

        int totalCount = await baseQuery.CountAsync(cancellationToken);

        IOrderedQueryable<Geography> sortedQuery = baseQuery.OrderBy(g => g.Name);
        int skip = (request.Page - 1) * request.PageSize;
        List<Geography> geographies = await sortedQuery
            .Skip(skip)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new GetAllGeographiesQueryResult { Geographies = geographies, TotalCount = totalCount };
    }
}
