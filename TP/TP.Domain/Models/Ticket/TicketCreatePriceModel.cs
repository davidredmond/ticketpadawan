namespace TP.Domain.Models.Ticket
{
    public record TicketCreatePriceModel
    {
        public required string TierName { get; set; }
        public decimal Price { get; set; }
        public int TotalAvailable { get; set; }
    }
}
