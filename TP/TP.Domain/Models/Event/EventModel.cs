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

        public static EventModel CopyFromEvent(Database.Models.Event eventRecord) => new EventModel
        {
            Description = eventRecord.Description,
            EndTime = eventRecord.EndTime,
            Id = eventRecord.Id,
            Name = eventRecord.Name,
            StartTime = eventRecord.StartTime,
            TicketPrices = eventRecord.TicketingCapacities.Select(a => new TicketPriceModel
            {
                Price = a.Price,
                TierId = a.Id,
                TierName = a.Tier,
                TotalAvailable = a.Tickets.Count(b => b.IsSold == false)
            }).ToList(),
            Venue = new VenueModel
            {
                Address = eventRecord.Venue.Location.Address,
                City = eventRecord.Venue.Location.City,
                Country = eventRecord.Venue.Location.Country,
                Id = eventRecord.Venue.Id,
                Name = eventRecord.Venue.Name,
                PostalCode = eventRecord.Venue.Location.PostalCode,
                Region = eventRecord.Venue.Location.Region
            }
        };
    }
}
