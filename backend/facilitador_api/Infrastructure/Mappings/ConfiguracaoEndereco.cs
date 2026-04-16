using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoEndereco : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("address");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Pais)
                .HasColumnName("country")
                .IsRequired();

            builder.Property(e => e.Estado)
                .HasColumnName("state")
                .IsRequired();

            builder.Property(e => e.Cidade)
                .HasColumnName("city")
                .IsRequired();

            builder.Property(e => e.Bairro)
                .HasColumnName("neighborhood")
                .IsRequired();

            builder.Property(e => e.Rua)
                .HasColumnName("street")
                .IsRequired();

            builder.Property(e => e.Numero)
                .HasColumnName("number")
                .IsRequired();

            builder.Property(e => e.CEP)
                .HasColumnName("cep")
                .IsRequired();

            builder.Property(e => e.Ativo)
                .HasColumnName("active")
                .IsRequired();

            builder.Property(e => e.CriadoEm)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(e => e.ModificadoEm)
                .HasColumnName("updated_at")
                .IsRequired();
        }
    }
}