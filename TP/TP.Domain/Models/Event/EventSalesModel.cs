using TP.Domain.Models.Ticket;

namespace TP.Domain.Models.Event
{
    public record EventSalesModel
    {
        public required EventModel Event { get; set; }
        public int TotalTicketsSold { get; set; }
        public int TotalTickets { get; set; }
        public int TotalTicketsRemaining { get; set; }
        public decimal TotalRevenue { get; set; }
        public required IEnumerable<TicketSalesModel> TicketPrices { get; set; }
    }
}
