using TP.Domain.Models.Ticket;

namespace TP.Domain.Models.Event
{
    public record EventCreateModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int VenueId { get; set; }
        
        public required IEnumerable<TicketCreatePriceModel> TicketPrices { get; set; }
    }
}
