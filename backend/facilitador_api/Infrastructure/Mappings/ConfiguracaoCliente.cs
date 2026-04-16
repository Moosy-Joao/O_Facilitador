using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoCliente : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("client");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.HasOne(c => c.Empresa)
                .WithMany(c => c.Clientes)
                .HasForeignKey(c => c.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Nome)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(c => c.ResponsavelFiscal)
                .HasColumnName("financial_officer");

            builder.Property(c => c.Documento)
                .HasColumnName("document")
                .IsRequired();

            builder.Property(c => c.Telefone)
                .HasColumnName("phone");

            builder.HasOne(c => c.Endereco)
                .WithOne()
                .HasForeignKey<Cliente>(c => c.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Saldo)
                .HasColumnName("balance")
                .IsRequired();

            builder.Property(c => c.LimiteCredito)
                .HasColumnName("credit_limit")
                .IsRequired();

            builder.Property(c => c.Nota)
                .HasColumnName("score");

            builder.Property(c => c.Ativo)
                .HasColumnName("active")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.CriadoEm)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(c => c.ModificadoEm)
                .HasColumnName("updated_at")
                .IsRequired();
        }
    }
}
