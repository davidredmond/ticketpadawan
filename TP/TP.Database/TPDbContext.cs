using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TP.Database.Models;

namespace TP.Database
{
    public class TPDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public TPDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Ticket> Tickets{ get; set; }
        public DbSet<TicketCapacity> TicketCapacities { get; set; }
        public DbSet<Venue> Venues { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(_configuration.GetConnectionString("TPDatabase"));

#if DEBUG
            options.LogTo(Console.WriteLine);
#endif
        }
    }
}
