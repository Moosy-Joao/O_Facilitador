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
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ValorPagamento)
                .HasColumnName("valor_pagamento")
                .IsRequired();

            builder.Property(e => e.Observacao)
                .HasColumnName("observacao")
                .IsRequired();

            builder.Property(e => e.DataPagamento)
                .HasColumnName("data_pagamento")
                .IsRequired();

            builder.Property(e => e.ClienteId)
                .HasColumnName("cliente_id")
                .IsRequired();

            builder.Property(e => e.EmpresaId)
                .HasColumnName("empresa_id")
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

            // Configurações de relacionamento
            builder.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.ClienteId);

            builder.HasOne(e => e.Empresa)
                .WithMany()
                .HasForeignKey(e => e.EmpresaId);
        }
    }
}