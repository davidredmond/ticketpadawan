using System.ComponentModel.DataAnnotations.Schema;

namespace TP.Database.Models
{
    [Table("Venues")]
    public class Venue : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public required string Name { get; set; }
        
        public required Location Location { get; set; }
    }
}
