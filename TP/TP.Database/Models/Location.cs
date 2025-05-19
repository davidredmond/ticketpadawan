using System.ComponentModel.DataAnnotations.Schema;

namespace TP.Database.Models
{
    [Table("Locations")]
    public class Location : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Region { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }
    }
}