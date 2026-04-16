using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoEmpresa : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("company");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nome)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(e => e.CNPJ)
                .HasColumnName("cnpj")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(e => e.Telefone)
                .HasColumnName("phone");

            builder.HasOne(e => e.Endereco)
                .WithOne()
                .HasForeignKey<Empresa>(c => c.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Ativo)
                .HasColumnName("active")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(e => e.CriadoEm)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(e => e.ModificadoEm)
                .HasColumnName("updated_at")
                .IsRequired();
        }
    }
}
