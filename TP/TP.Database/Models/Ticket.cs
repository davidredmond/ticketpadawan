using System.ComponentModel.DataAnnotations.Schema;

namespace TP.Database.Models
{
    [Table("Tickets")]
    public class Ticket : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public required string Description { get; set; }
        public bool IsSold { get; set; }
        public DateTime SoldTime { get; set; }
    }
}