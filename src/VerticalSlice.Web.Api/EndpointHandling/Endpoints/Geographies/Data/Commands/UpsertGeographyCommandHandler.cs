using Kommand.Abstractions;
using Microsoft.EntityFrameworkCore;
using VerticalSlice.Web.Api.Data;
using VerticalSlice.Web.Api.Model;

namespace VerticalSlice.Web.Api.EndpointHandling.Endpoints.Geographies.Data.Commands;

public class UpsertGeographyCommandResult
{
    public int GeographyId { get; set; }
    public bool IsNew { get; set; }
}

public class UpsertGeographyCommandHandler(VerticalSliceDataContext dataContext)
    : ICommandHandler<UpsertGeographyCommand, UpsertGeographyCommandResult>
{
    private readonly VerticalSliceDataContext _dataContext =
        dataContext ?? throw new ArgumentNullException(nameof(dataContext));

    public async Task<UpsertGeographyCommandResult> HandleAsync(UpsertGeographyCommand request, CancellationToken cancellationToken)
    {
        bool isNew = false;
        Geography? geography = null;

        if (request.GeographyId.HasValue && request.GeographyId.Value > 0)
        {
            geography = await _dataContext.Geographies
                .FirstOrDefaultAsync(g => g.GeographyId == request.GeographyId.Value, cancellationToken);
        }

        if (geography == null)
        {
            geography = new Geography();
            await _dataContext.Geographies.AddAsync(geography, cancellationToken);
            isNew = true;
        }

        geography.Name = request.Name;
        geography.ShortCode = request.ShortCode;
        geography.GeoCodes = request.GeoCodes;
        geography.Region = request.Region;
        geography.SubRegion = request.SubRegion;
        geography.Capital = request.Capital;
        geography.Population = request.Population;
        geography.AreaKm2 = request.AreaKm2;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return new UpsertGeographyCommandResult { GeographyId = geography.GeographyId, IsNew = isNew };
    }
}
