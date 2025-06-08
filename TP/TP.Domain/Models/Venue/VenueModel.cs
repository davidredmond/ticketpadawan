namespace TP.Domain.Models.Venue
{
    public record VenueModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Region { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }

        public static VenueModel CopyFromVenue(Database.Models.Venue venue) => new VenueModel
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
