using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoCompra : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.ToTable("compra");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .IsRequired();

            builder.Property(c => c.Valor)
                .HasColumnName("valor")
                .IsRequired();

            builder.Property(c => c.Descricao)
                .HasColumnName("descricao")
                .IsRequired();

            builder.Property(c => c.NumeroNota)
                .HasColumnName("numero_nota");

            // Configurações de relacionamento cliente
            builder.Property(c => c.ClienteId)
                .HasColumnName("cliente_id")
                .IsRequired();

            builder.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurações de relacionamento empresa
            builder.Property(c => c.EmpresaId)
                .HasColumnName("empresa_id")
                .IsRequired();

            builder.HasOne(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Ativo)
                .HasColumnName("ativo")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(c => c.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();

            builder.Property(c => c.ModificadoEm)
                .HasColumnName("modificado_em")
                .IsRequired();
        }
    }
}
