using Microsoft.EntityFrameworkCore;
using TP.Database;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;

namespace TP.Domain.Queries.Report
{
    public record class GetSalesReportForEventQuery(int Id) : IQuery<WorkResult<EventSalesModel>> { }

    public class GetSalesReportForEventQueryHandler : IQueryHandler<GetSalesReportForEventQuery, WorkResult<EventSalesModel>>
    {
        private readonly TPDbContext _dbContext;

        public GetSalesReportForEventQueryHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<EventSalesModel>> HandleAsync(GetSalesReportForEventQuery command)
        {
            var dbEvent = await _dbContext.Events
                            .Include(a => a.TicketingCapacities)
                                .ThenInclude(a => a.Tickets)
                            .Include(a => a.Venue)
                                .ThenInclude(a => a.Location)
                            .FirstOrDefaultAsync(a => a.Id == command.Id);

            if (dbEvent == null)
            {
                return new WorkResult<EventSalesModel>(null)
                {
                    IsSuccess = false,
                    Message = "Event not found"
                };
            }

            var salesModel = new EventSalesModel
            {
                Event = EventModel.CopyFromEvent(dbEvent),
                TicketPrices = dbEvent.TicketingCapacities.Select(a => new Models.Ticket.TicketSalesModel
                {
                    Price = a.Price,
                    TierId = a.Id,
                    TierName = a.Tier,
                    TotalTickets = a.Tickets.Count(),
                    TotalTicketsRemaining = a.Tickets.Count(b => b.IsSold == false),
                    TotalTicketsSold = a.Tickets.Count(b => b.IsSold),
                    Revenue = a.Price * a.Tickets.Count(b => b.IsSold)
                }).ToList(),
            };
            salesModel.TotalTickets = salesModel.TicketPrices.Sum(a => a.TotalTickets);
            salesModel.TotalTicketsSold = salesModel.TicketPrices.Sum(a => a.TotalTicketsSold);
            salesModel.TotalTicketsRemaining = salesModel.TotalTickets - salesModel.TotalTicketsSold;
            salesModel.TotalRevenue = salesModel.TicketPrices.Sum(a => a.Revenue);
            return new WorkResult<EventSalesModel>(salesModel)
            {
                IsSuccess = true,
                Message = "Event sales report"
            };
        }
    }
}
