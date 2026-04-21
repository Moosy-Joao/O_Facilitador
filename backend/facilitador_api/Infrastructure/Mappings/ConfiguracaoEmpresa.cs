using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoEmpresa : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("empresa");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Nome)
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(e => e.CNPJ)
                .HasColumnName("cnpj")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(e => e.Telefone)
                .HasColumnName("telefone");

            // Configura o relacionamento com Endereco
            builder.HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.EnderecoId)
                .HasColumnName("endereco_id");

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
