namespace TP.Domain.Models.Ticket
{
    public record TicketPriceModel
    {
        public int TierId { get; set; }
        public required string TierName { get; set; }
        public decimal Price { get; set; }
        public int TotalAvailable { get; set; }
    }
}
