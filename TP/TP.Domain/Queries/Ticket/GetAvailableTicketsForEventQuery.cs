using Microsoft.EntityFrameworkCore;
using TP.Database;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;

namespace TP.Domain.Queries.Ticket
{
    public record GetAvailableTicketsForEventQuery(int EventId) : IQuery<WorkResult<IEnumerable<TicketPriceModel>>> { }

    public class GetAvailableTicketsForEventQueryHandler : IQueryHandler<GetAvailableTicketsForEventQuery, WorkResult<IEnumerable<TicketPriceModel>>>
    {
        private readonly TPDbContext _dbContext;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public GetAvailableTicketsForEventQueryHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<IEnumerable<TicketPriceModel>>> HandleAsync(GetAvailableTicketsForEventQuery command)
        {
            var dbEvent = await _dbContext.Events
                .Include(a => a.TicketingCapacities)
                    .ThenInclude(a => a.Tickets)
                .Include(a => a.Venue)
                .ThenInclude(a => a.Location)
                .FirstOrDefaultAsync(a => a.Id == command.EventId);

            if (dbEvent == null)
            {
                return new WorkResult<IEnumerable<TicketPriceModel>>(null)
                {
                    IsSuccess = false,
                    Message = "Event not found"
                };
            }

            if (dbEvent.IsCancelled || dbEvent.IsDeleted)
            {
                return new WorkResult<IEnumerable<TicketPriceModel>>(null)
                {
                    IsSuccess = false,
                    Message = "Event is no longer available for ticket purchases"
                };
            }

            await _semaphore.WaitAsync();

            try
            {
                var ticketsAvailable = dbEvent.TicketingCapacities.Select(a => new TicketPriceModel
                {
                    TierId = a.Id,
                    Price = a.Price,
                    TierName = a.Tier,
                    TotalAvailable = a.Tickets.Count(a => a.IsSold == false)
                }).ToList();
                return new WorkResult<IEnumerable<TicketPriceModel>>(ticketsAvailable)
                {
                    IsSuccess = true,
                    Message = "Tickets available successfully"
                };
            }
            catch (Exception ex)
            {
                return new WorkResult<IEnumerable<TicketPriceModel>>(null)
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
