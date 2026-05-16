using facilitador_domain.Domain.DTOs;
using facilitador_api.Domain.Entities;

namespace facilitador_api.Application.Mapping
{
    public static class EmpresaMapping
    {
        public static EmpresaResponseDTO ToResponseDTO(this Empresa empresa) => new()
        {
            Id = empresa.Id,
            Nome = empresa.Nome,
            CNPJ = empresa.CNPJ,
            Email = empresa.Email,
            Telefone = empresa.Telefone,
            Endereco = empresa.Endereco?.ToResponseDTO(),
            Ativo = empresa.Ativo,
            CriadoEm = empresa.CriadoEm,
            ModificadoEm = empresa.ModificadoEm
        };
    }
}
