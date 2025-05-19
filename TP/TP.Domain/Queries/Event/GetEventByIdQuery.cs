using Microsoft.EntityFrameworkCore;
using TP.Database;
using TP.Domain.Models;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;

namespace TP.Domain.Queries.Event
{
    public record GetEventByIdQuery(int Id) : IQuery<WorkResult<EventModel>> { }

    public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, WorkResult<EventModel>>
    {
        private readonly TPDbContext _dbContext;
        
        public GetEventByIdQueryHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<EventModel>> HandleAsync(GetEventByIdQuery command)
        {
            var dbEvent = await _dbContext.Events
                .Include(a => a.TicketingCapacities)
                    .ThenInclude(a => a.Tickets)
                .Include(a => a.Venue)
                    .ThenInclude(a => a.Location)
                .FirstOrDefaultAsync(a => a.Id == command.Id);

            if (dbEvent == null)
            {
                return new WorkResult<EventModel>(null)
                {
                    IsSuccess = false,
                    Message = "Event not found"
                };
            }

            return new WorkResult<EventModel>(MappingHelper.MapEventToEventModel(dbEvent))
            {
                IsSuccess = true,
                Message = "Event retrieved successfully"
            };
        }
    }
}
