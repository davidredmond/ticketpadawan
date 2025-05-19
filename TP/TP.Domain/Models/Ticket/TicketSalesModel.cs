namespace TP.Domain.Models.Ticket
{
    public record TicketSalesModel
    {
        public int TierId { get; set; }
        public required string TierName { get; set; }
        public decimal Price { get; set; }
        public decimal Revenue { get; set; }
        public int TotalTicketsSold { get; set; }
        public int TotalTickets { get; set; }
        public int TotalTicketsRemaining { get; set; }
    }
}
