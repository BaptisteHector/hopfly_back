using Microsoft.EntityFrameworkCore;

namespace HopflyApi.Models
{
    public class HopflyContext : DbContext
    {
        public HopflyContext(DbContextOptions<HopflyContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Logement> Logements { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}