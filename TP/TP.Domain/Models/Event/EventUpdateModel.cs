namespace TP.Domain.Models.Event
{
    public class EventUpdateModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDeleted { get; set; }
    }
}
