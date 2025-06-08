using Microsoft.EntityFrameworkCore;
using TP.Database;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;

namespace TP.Domain.Queries.Event
{
    public record GetAllActiveEventsQuery() : IQuery<WorkResult<IEnumerable<EventModel>>> { }

    public class GetAllActiveEventsQueryHandler : IQueryHandler<GetAllActiveEventsQuery, WorkResult<IEnumerable<EventModel>>>
    {
        private readonly TPDbContext _dbContext;

        public GetAllActiveEventsQueryHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<IEnumerable<EventModel>>> HandleAsync(GetAllActiveEventsQuery command)
        {
            var dbEvents = await _dbContext.Events
                .Include(a => a.TicketingCapacities)
                    .ThenInclude(a => a.Tickets)
                .Include(a => a.Venue)
                    .ThenInclude(a => a.Location)
                .Where(a => a.IsCancelled == false && a.IsDeleted == false).ToListAsync();
            return new WorkResult<IEnumerable<EventModel>>(dbEvents.Select(EventModel.CopyFromEvent))
            {
                IsSuccess = true,
                Message = $"{dbEvents.Count} events retrieved successfully"
            };
        }
    }
}
