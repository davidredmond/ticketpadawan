using Microsoft.EntityFrameworkCore;
using TP.Database;
using TP.Database.Models;
using TP.Domain.Models;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;

namespace TP.Domain.Commands.Event
{
    public record CreateEventCommand(EventCreateModel Model) : ICommand<WorkResult<EventModel>> { }

    public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, WorkResult<EventModel>>
    {
        private readonly TPDbContext _dbContext;
        
        public CreateEventCommandHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<EventModel>> HandleAsync(CreateEventCommand command)
        {
            var dbVenue = await _dbContext.Venues
                .Include(a => a.Location)
                .FirstOrDefaultAsync(a => a.Id == command.Model.VenueId);

            if (dbVenue == null)
            {
                return new WorkResult<EventModel>(null)
                {
                    IsSuccess = false,
                    Message = "Venue not found"
                };
            }

            var ticketCapacities = new List<TicketCapacity>();

            foreach (var ticket in command.Model.TicketPrices)
            {
                var tickets = new List<Database.Models.Ticket>();

                for (var counter = 1; counter <= ticket.TotalAvailable; counter++)
                {
                    tickets.Add(new Database.Models.Ticket
                    {
                        Created = DateTime.UtcNow,
                        Description = $"TICKET_{counter.ToString().PadLeft(4, '0')}",
                        Updated = DateTime.UtcNow,
                    });
                }

                ticketCapacities.Add(new TicketCapacity
                {
                    Created = DateTime.UtcNow,
                    Price = ticket.Price,
                    Tickets = tickets,
                    Tier = ticket.TierName,
                    Updated = DateTime.UtcNow,
                });
            }

            var dbEvent = new Database.Models.Event
            {
                Description = command.Model.Description,
                Created = DateTime.UtcNow,
                EndTime = command.Model.EndTime,
                IsCancelled = false,
                IsDeleted = false,
                Name = command.Model.Name,
                StartTime = command.Model.StartTime,
                TicketingCapacities = ticketCapacities,
                Updated = DateTime.UtcNow,
                Venue = dbVenue
            };

            try
            {
                await _dbContext.Events.AddAsync(dbEvent);
                await _dbContext.SaveChangesAsync();
                return new WorkResult<EventModel>(MappingHelper.MapEventToEventModel(dbEvent))
                {
                    IsSuccess = true,
                    Message = "Event created successfully"
                };
            }
            catch (Exception ex)
            {
                return new WorkResult<EventModel>(null)
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
