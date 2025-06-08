using TP.Domain.Models.Result;
using TP.Domain.Models.Venue;
using TP.Database;
using Microsoft.EntityFrameworkCore;

namespace TP.Domain.Queries.Venue
{
    public record GetAllVenuesQuery() : IQuery<WorkResult<IEnumerable<VenueModel>>> { }

    public class GetAllVenuesQueryHandler : IQueryHandler<GetAllVenuesQuery, WorkResult<IEnumerable<VenueModel>>>
    {
        private readonly TPDbContext _dbContext;

        public GetAllVenuesQueryHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<IEnumerable<VenueModel>>> HandleAsync(GetAllVenuesQuery command)
        {
            var venues = await _dbContext.Venues
                .Include(a => a.Location)
                .ToListAsync();
            return new WorkResult<IEnumerable<VenueModel>>(venues.Select(VenueModel.CopyFromVenue))
            {
                IsSuccess = true,
                Message = $"{venues.Count} venues retrieved successfully"
            };
        }
    }
}
