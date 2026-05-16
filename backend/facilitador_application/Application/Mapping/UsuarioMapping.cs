using facilitador_domain.Domain.DTOs;
using facilitador_api.Domain.Entities;

namespace facilitador_api.Application.Mapping
{
    public static class UsuarioMapping
    {
        public static UsuarioResponseDTO ToResponseDTO(this Usuario usuario)
        {
            if (usuario == null) return null!;

            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Cargo = usuario.Cargo,
                Imagem = usuario.Imagem,
                EmpresaId = usuario.EmpresaId,
                Ativo = usuario.Ativo,
                CriadoEm = usuario.CriadoEm,
                ModificadoEm = usuario.ModificadoEm
            };
        }
    }
}
