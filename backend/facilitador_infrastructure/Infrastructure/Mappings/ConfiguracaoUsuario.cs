using facilitador_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace facilitador_api.Infrastructure.Mappings
{
    public class ConfiguracaoUsuario : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            // Configura o relacionamento com a entidade Empresa
            builder.HasOne(u => u.Empresa)
                .WithMany()
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.EmpresaId)
                .HasColumnName("empresa_id");

            builder.Property(u => u.Nome)
                .HasColumnName("nome")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(u => u.Senha)
                .HasColumnName("senha")
                .IsRequired();

            builder.Property(u => u.Cargo)
                .HasColumnName("cargo")
                .IsRequired();

            builder.Property(u => u.Imagem)
                .HasColumnName("image");

            builder.Property(u => u.Ativo)
                .HasColumnName("ativo")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(u => u.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();

            builder.Property(u => u.ModificadoEm)
                .HasColumnName("modificado_em")
                .IsRequired();
        }
    }
}
