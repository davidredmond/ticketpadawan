using Microsoft.EntityFrameworkCore;
using TP.Database;
using TP.Domain.Models.Result;

namespace TP.Domain.Commands.Ticket
{
    public record class RefundTicketCommand(int EventId, int TicketId) : ICommand<WorkResult<bool>> { }

    public class RefundTicketCommandHandler : ICommandHandler<RefundTicketCommand, WorkResult<bool>>
    {
        private readonly TPDbContext _dbContext;

        public RefundTicketCommandHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<bool>> HandleAsync(RefundTicketCommand command)
        {
            var dbEvent = await _dbContext.Events
                .Include(a => a.TicketingCapacities)
                    .ThenInclude(a => a.Tickets)
                .FirstOrDefaultAsync(a => a.Id == command.EventId);

            if (dbEvent == null)
            {
                return new WorkResult<bool>(false)
                {
                    IsSuccess = false,
                    Message = "Event not found"
                };
            }

            var dbTicket = dbEvent.TicketingCapacities.SelectMany(a => a.Tickets) .FirstOrDefault(a => a.Id == command.TicketId);

            if (dbTicket == null)
            {
                return new WorkResult<bool>(false)
                {
                    IsSuccess = false,
                    Message = "Ticket not found"
                };
            }

            if (dbTicket.IsSold == false)
            {
                return new WorkResult<bool>(false)
                {
                    IsSuccess = false,
                    Message = "Ticket is not sold"
                };
            }

            dbTicket.IsSold = false;
            dbTicket.Updated = DateTime.UtcNow;
            dbTicket.SoldTime = null;

            try
            {
                await _dbContext.SaveChangesAsync();
                return new WorkResult<bool>(true)
                {
                    IsSuccess = true,
                    Message = "Ticket refunded successfully"
                };
            }
            catch (Exception ex)
            {
                return new WorkResult<bool>(false)
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
