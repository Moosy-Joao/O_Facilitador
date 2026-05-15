using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoEndereco : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("endereco");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Pais)
                .HasColumnName("pais")
                .IsRequired();

            builder.Property(e => e.Estado)
                .HasColumnName("estado")
                .IsRequired();

            builder.Property(e => e.Cidade)
                .HasColumnName("cidade")
                .IsRequired();

            builder.Property(e => e.Bairro)
                .HasColumnName("bairro")
                .IsRequired();

            builder.Property(e => e.Rua)
                .HasColumnName("rua")
                .IsRequired();

            builder.Property(e => e.Numero)
                .HasColumnName("numero")
                .IsRequired();

            builder.Property(e => e.CEP)
                .HasColumnName("cep")
                .IsRequired();

            builder.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();

            builder.Property(e => e.ModificadoEm)
                .HasColumnName("modificado_em")
                .IsRequired();
        }
    }
}