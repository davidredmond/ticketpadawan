using TP.Domain.Models.Event;
using TP.Domain.Models.Ticket;
using TP.Domain.Models.Venue;

namespace TP.Domain.Models
{
    public static class MappingHelper
    {
        public static TicketModel MapTicketToTicketModel(Database.Models.Ticket ticket, Database.Models.Event eventModel) => new TicketModel
        {
            Id = ticket.Id,
            SoldTime = ticket.SoldTime
        };

        public static EventModel MapEventToEventModel(Database.Models.Event dbEvent) => new EventModel
        {
            Description = dbEvent.Description,
            EndTime = dbEvent.EndTime,
            Id = dbEvent.Id,
            Name = dbEvent.Name,
            StartTime = dbEvent.StartTime,
            TicketPrices = dbEvent.TicketingCapacities.Select(a => new TicketPriceModel
            {
                Price = a.Price,
                TierId = a.Id,
                TierName = a.Tier,
                TotalAvailable = a.Tickets.Count(b => b.IsSold == false)
            }).ToList(),
            Venue = new VenueModel
            {
                Address = dbEvent.Venue.Location.Address,
                City = dbEvent.Venue.Location.City,
                Country = dbEvent.Venue.Location.Country,
                Id = dbEvent.Venue.Id,
                Name = dbEvent.Venue.Name,
                PostalCode = dbEvent.Venue.Location.PostalCode,
                Region = dbEvent.Venue.Location.Region
            }
        };

        public static VenueModel MapVenueToVenueModel(Database.Models.Venue venue) => new VenueModel
        {
            Address = venue.Location.Address,
            City = venue.Location.City,
            Country = venue.Location.Country,
            Id = venue.Id,
            Name = venue.Name,
            PostalCode = venue.Location.PostalCode,
            Region = venue.Location.Region
        };
    }
}
