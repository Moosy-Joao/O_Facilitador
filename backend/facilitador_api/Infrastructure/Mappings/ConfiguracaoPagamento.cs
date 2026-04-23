using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoPagamento : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("pagamento");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.ClienteId)
                .HasColumnName("cliente_id")
                .IsRequired();

            builder.Property(e => e.EmpresaId)
                .HasColumnName("empresa_id")
                .IsRequired();

            builder.Property(e => e.Observacao)
                .HasColumnName("observacao")
                .IsRequired(false);

            builder.Property(e => e.PagamentoValor)
                .HasColumnName("pagamento_value")
                .IsRequired();

            builder.Property(e => e.PagamentoData)
                .HasColumnName("pagamento_data")
                .IsRequired(false);

            builder.Property(e => e.Ativo)
                .HasColumnName("ativo")
                .IsRequired();

            builder.Property(e => e.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();

            builder.Property(e => e.ModificadoEm)
                .HasColumnName("modificado_em")
                .IsRequired(false);

            // FK
            builder.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId);

            builder.HasOne(e => e.Empresa)
                .WithMany()
                .HasForeignKey(e => e.EmpresaId);
        }
    }
}