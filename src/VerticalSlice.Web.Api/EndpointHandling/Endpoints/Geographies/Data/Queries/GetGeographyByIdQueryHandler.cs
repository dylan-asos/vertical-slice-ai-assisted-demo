using Kommand.Abstractions;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Web.Api.Data;
using VerticalSlice.Web.Api.Model;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Queries;

public class GetGeographyByIdQueryResult
{
    public Geography? Geography { get; set; }
}

public class GetGeographyByIdQueryHandler(VerticalSliceDataContext dataContext)
    : IQueryHandler<GetGeographyByIdQuery, GetGeographyByIdQueryResult>
{
    private readonly VerticalSliceDataContext _dataContext =
        dataContext ?? throw new ArgumentNullException(nameof(dataContext));

    public async Task<GetGeographyByIdQueryResult> HandleAsync(GetGeographyByIdQuery request, CancellationToken cancellationToken)
    {
        Geography? geography = await _dataContext.Geographies
            .FirstOrDefaultAsync(g => g.GeographyId == request.GeographyId, cancellationToken);

        return new GetGeographyByIdQueryResult { Geography = geography };
    }
}
