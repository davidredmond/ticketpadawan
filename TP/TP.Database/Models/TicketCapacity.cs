using System.ComponentModel.DataAnnotations.Schema;

namespace TP.Database.Models
{
    [Table("TicketCapacities")]
    public class TicketCapacity : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public decimal Price { get;set; }
        public required string Tier { get; set; }

        public required IEnumerable<Ticket> Tickets { get; set; }
    }
}