using Microsoft.EntityFrameworkCore;
using facilitador_api.Domain.Entities;

namespace facilitador_api.Infrastructure.DB
{
    public class ConnectionContext : DbContext
    {
        public ConnectionContext(DbContextOptions<ConnectionContext> options)
            : base(options)
        {
        }

        //public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        //public DbSet<CompraFiado> ComprasFiado { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(entity =>
            {
                // 🔹 Nome da tabela (EXATO do banco)
                entity.ToTable("Clientes");

                // 🔹 Mapeamento das colunas (case-sensitive)
                entity.Property(c => c.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(c => c.Nome)
                    .HasColumnName("nome")
                    .IsRequired();

                entity.Property(c => c.Documento)
                    .HasColumnName("documento")
                    .IsRequired();

                entity.Property(c => c.Telefone)
                    .HasColumnName("telefone")
                    .IsRequired();

                entity.Property<string?>("Endereco")
                    .HasColumnName("endereco");

                entity.Property<decimal>("LimiteCredito")
                    .HasColumnName("limite_credito");

                entity.Property<bool>("Ativo")
                    .HasColumnName("ativo");

                entity.Property<DateTime>("CriadoEm")
                    .HasColumnName("criado_em");

                // 🔹 Índice único
                entity.HasIndex(c => c.Documento)
                    .IsUnique();
            });
        }
    }
}