namespace TP.Domain.Models.Ticket
{
    public record TicketModel
    {
        public int Id { get; set; }
        public DateTime? SoldTime { get; set; }

        public static TicketModel CopyFromTicket(Database.Models.Ticket ticket)
        {
            return new TicketModel()
            {
                Id = ticket.Id,
                SoldTime = ticket.SoldTime,
            };
        }
    }
}
