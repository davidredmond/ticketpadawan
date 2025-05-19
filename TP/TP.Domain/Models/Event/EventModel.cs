using TP.Domain.Models.Ticket;
using TP.Domain.Models.Venue;

namespace TP.Domain.Models.Event
{
    public record EventModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public required VenueModel Venue { get; set; }
        public required IEnumerable<TicketPriceModel> TicketPrices { get; set; }
    }
}
