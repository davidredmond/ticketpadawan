namespace TP.Domain.Models.Ticket
{
    public record TicketModel
    {
        public int Id { get; set; }
        public DateTime? SoldTime { get; set; }
    }
}
