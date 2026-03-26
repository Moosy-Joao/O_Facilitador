using facilitador_api.Model;
using Microsoft.EntityFrameworkCore;

namespace facilitador_api.Infrastructure.DB
{
    public class ConnectionContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<Client> Clients { get; private set; }
        public DbSet<Address> Addresses { get; private set; }
        public DbSet<Purchase> Purchases { get; private set; }
        public DbSet<Payment> Payments { get; private set; }

        public ConnectionContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
