using facilitador_api.Model;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.DB
{
    public class ConnectionContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public ConnectionContext(DbContextOptions<ConnectionContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
