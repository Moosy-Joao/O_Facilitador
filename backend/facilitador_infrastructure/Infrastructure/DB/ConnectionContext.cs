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
            modelBuilder.Entity<Usuario>(builder =>
            {
                builder.ToTable("usuario");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.Id)
                    .HasColumnName("id");

                builder.Property(e => e.Nome)
                    .HasColumnName("nome");

                builder.Property(e => e.Email)
                    .HasColumnName("email");

                builder.Property(e => e.Senha)
                    .HasColumnName("senha");

                builder.Property(e => e.Cargo)
                    .HasColumnName("cargo");


                builder.Property(e => e.EmpresaId)
                    .HasColumnName("empresa_id");
            });

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
