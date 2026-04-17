using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoCliente : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("cliente");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.HasOne(c => c.Empresa)
                .WithMany(c => c.Clientes)
                .HasForeignKey(c => c.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Nome)
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(c => c.Documento)
                .HasColumnName("documento")
                .IsRequired();

            builder.Property(c => c.Telefone)
                .HasColumnName("telefone");

            builder.HasOne(c => c.Endereco)
                .WithOne()
                .HasForeignKey<Cliente>(c => c.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Saldo)
                .HasColumnName("saldo")
                .IsRequired();

            builder.Property(c => c.LimiteCredito)
                .HasColumnName("limite_credito")
                .IsRequired();

            builder.Property(c => c.Nota)
                .HasColumnName("nota");

            builder.Property(c => c.Ativo)
                .HasColumnName("ativo")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();

            builder.Property(c => c.ModificadoEm)
                .HasColumnName("modificado_em")
                .IsRequired();
        }
    }
}
