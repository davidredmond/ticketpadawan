using System.ComponentModel.DataAnnotations.Schema;

namespace TP.Database.Models
{
    [Table("Events")]
    public class Event : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public required string Name { get; set; }
        public required string Description {  get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCancelled { get; set; }
        
        public required Venue Venue { get; set; }
        public required IEnumerable<TicketCapacity> TicketingCapacities { get; set; }
    }
}
