using Microsoft.EntityFrameworkCore;
using TP.Database;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;

namespace TP.Domain.Commands.Ticket
{
    public record class PurchaseTicketCommand(int EventId, int TierId, int Count) : ICommand<WorkResult<IEnumerable<TicketModel>>> { }

    public class PurchaseTicketCommandHandler : ICommandHandler<PurchaseTicketCommand, WorkResult<IEnumerable<TicketModel>>>
    {
        private readonly TPDbContext _dbContext;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public PurchaseTicketCommandHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<IEnumerable<TicketModel>>> HandleAsync(PurchaseTicketCommand command)
        {
            var dbEvent = await _dbContext.Events
                .Include(a => a.TicketingCapacities)
                    .ThenInclude(a => a.Tickets)
                .Include(a => a.Venue)
                    .ThenInclude(a => a.Location)
                .FirstOrDefaultAsync(a => a.Id == command.EventId);

            if (dbEvent == null)
            {
                return new WorkResult<IEnumerable<TicketModel>>(null)
                {
                    IsSuccess = false,
                    Message = "Event not found"
                };
            }

            if (dbEvent.IsCancelled || dbEvent.IsDeleted)
            {
                return new WorkResult<IEnumerable<TicketModel>>(null)
                {
                    IsSuccess = false,
                    Message = "Event is no longer available for ticket purchases"
                };
            }

            await _semaphore.WaitAsync();
            var ticketsAvailable = dbEvent.TicketingCapacities.First(a => a.Id == command.TierId).Tickets.Count(a => a.IsSold == false);

            if (ticketsAvailable <= command.Count)
            {
                _semaphore.Release();
                return new WorkResult<IEnumerable<TicketModel>>(null)
                {
                    IsSuccess = false,
                    Message = "Not enough tickets available for tier requested"
                };
            }

            var tickets = dbEvent.TicketingCapacities.First(a => a.Id == command.TierId).Tickets.Where(a => a.IsSold == false).Take(command.Count).ToList();

            foreach (var ticket in tickets)
            {
                ticket.IsSold = true;
                ticket.Updated = DateTime.UtcNow;
                ticket.SoldTime = DateTime.UtcNow;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return new WorkResult<IEnumerable<TicketModel>>(tickets.Select(TicketModel.CopyFromTicket).ToList())
                {
                    IsSuccess = true,
                    Message = "Tickets purchased successfully"
                };
            }
            catch (Exception ex)
            {
                return new WorkResult<IEnumerable<TicketModel>>(null)
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
