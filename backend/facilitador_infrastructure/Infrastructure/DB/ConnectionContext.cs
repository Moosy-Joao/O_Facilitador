
using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace facilitador_api.Infrastructure.DB
{
    public class ConnectionContext : DbContext
    {
        public ConnectionContext()
        {
        }

        public ConnectionContext(DbContextOptions<ConnectionContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}